using JourneyMate.Application.ServicesApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace JourneyMate.Presentation.Controllers;
[Authorize(Roles = "Super,Admin")]
public class TourismCompanyRequestController : BaseController
{

    private readonly ITourismCompanyRequestService _tourismCompanyRequestService;
    private readonly ILogger<ITourismCompanyRequestService> _logger;

    public TourismCompanyRequestController(ITourismCompanyRequestService tourismCompanyRequestService, ILogger<ITourismCompanyRequestService> logger)
    {
        _tourismCompanyRequestService = tourismCompanyRequestService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<TourismCompanyRequestDto>? tourismCompanyRequests = _tourismCompanyRequestService.GetAllTourismCompanyRequestAsync().Result;
        return View(tourismCompanyRequests);
    }

    public async Task<IActionResult> ApproveRequest(int id)
    {

        try
        {
            var success = await _tourismCompanyRequestService.UpdateTourismCompanyRequestAsync(id); ;
            return HandleOperationResult(success, "Error update TourismCompanyRequest.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while update TourismCompanyRequest {id}");
            return StatusCode(500, "Internal server error.");
        }

    }
    public async Task<IActionResult> Delete(int id)
    {

        try
        {
            var success = await _tourismCompanyRequestService.DeleteTourismCompanyRequestAsync(id); ;
            return HandleOperationResult(success, "Error deleting  TourismCompanyRequest.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting TourismCompanyRequest {id}");
            return StatusCode(500, "Internal server error.");
        }
    }

}
