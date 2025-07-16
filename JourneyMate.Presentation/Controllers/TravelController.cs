using JourneyMate.Core.Models.Company_Aggregation;
using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;
using JourneyMate.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;


namespace JourneyMate.Presentation.Controllers
{
    [Authorize(Roles = "Company")]
    public class TravelController : BaseController
    {
        private readonly ITravelService _travelService;
        private readonly IAiTravelService _aiTravelService;
        private readonly ILogger<TravelController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment; // Inject IWebHostEnvironment

        public TravelController(
            ITravelService travelService,
            ILogger<TravelController> logger,
            IWebHostEnvironment webHostEnvironment,
            IAiTravelService aiTravelService)
        {
            _travelService = travelService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _aiTravelService = aiTravelService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User not found in session.");
                    return RedirectToAction("Index", "User");
                }

                var travels = await _travelService.GetTravelsByCompanyAsync(userId);
                return View(travels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching travels.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var travelVm = await _travelService.GetTravelByIdAsync(id);
                return HandleNullResult(travelVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching travel details for ID: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
           
            var categories =await _travelService.GetCategories();
            var AllCategory = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            });
            var viewModel = new TravelVm
            {
                Categories = AllCategory
            };

            return View(viewModel);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TravelVm travelVm)
        {
            if (!ModelState.IsValid)
            {
                var Categories = await _travelService.GetCategories();
                travelVm.Categories = Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName
                });
                return View(travelVm);
            }
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                CreateTravelVmParamter TravelVmParamter = new CreateTravelVmParamter()
                {
                    travelVm = travelVm,
                    userId = userId,
                    images=travelVm.IFormFileImages,
                    ProfileImage=travelVm.IFormFileProfileImage,
                    CoverImage=travelVm.IFormFileCoverImage
                };
                var success = await _travelService.CreateTravelAsync( TravelVmParamter);
                return HandleOperationResult(success, "Error creating travel.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating travel.");
                return StatusCode(500, "Internal server error.");
            }
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var travelVm = await _travelService.GetTravelByIdAsync(id);
                if (travelVm == null)
                {
                    return NotFound();
                }

                // ✅ CRITICAL: Populate the Categories dropdown for the edit view
                var categories = await _travelService.GetCategories();
                travelVm.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName,
                    Selected = c.Id == travelVm.CategoryId // ✅ Pre-select the current category
                });

                return View(travelVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while loading update view for travel ID: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TravelVm travelVm, List<int> ImagesToRemove)
        {
            if (!ModelState.IsValid)
            {
                // ✅ CRITICAL: Re-populate Categories when validation fails
                var categories = await _travelService.GetCategories();
                travelVm.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName,
                    Selected = c.Id == travelVm.CategoryId
                });

                return View(travelVm);
            }

            try
            {
                var success = await _travelService.UpdateTravelAsync(id, travelVm, ImagesToRemove, travelVm.IFormFileImages);
                return HandleOperationResult(success, "Error updating travel.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating travel {id}");
                return StatusCode(500, "Internal server error.");
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var travelVm = await _travelService.GetTravelByIdAsync(id);
                return HandleNullResult(travelVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while loading delete view for travel ID: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _travelService.DeleteTravelAsync(id);
                return HandleOperationResult(success, "Error deleting travel.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting travel {id}");
                return StatusCode(500, "Internal server error.");
            }
        }
        #endregion

        
    }
}
