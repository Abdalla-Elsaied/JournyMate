using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static JourneyMate.Presentation.Controllers.CompanyDashController;

namespace JourneyMate.Presentation.Controllers;
[Authorize(Roles = "Company")]

public partial class CompanyDashController : BaseController
{
    private readonly ICompanyDashService _companyService;
    private readonly ILogger<CompanyDashController> _logger;

    public CompanyDashController(ICompanyDashService companyService, ILogger<CompanyDashController> logger)
    {
        _companyService = companyService;
        _logger = logger;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            string? userId = HttpContext.Session.GetString("CurrentCompany");

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User not found in session.");
                return BadRequest();
            }

            var company = await _companyService.GetCompanyByUserIdAsync(userId);
            return HandleNullResult(company, "Error fetching company data.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching company dashboard data.");
            return StatusCode(500, "Internal server error.");
        }
    }
    public async Task<IActionResult> Edit()
    {
        string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Edit action received null or empty ID.");
                return BadRequest("Invalid request.");
            }

            var company = await _companyService.GetCompanyByUserIdAsync(id);
            return HandleNullResult(company, "Error loading edit view.", "Company or User not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while loading edit view for ID: {id}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CompanyVm obj)
    {
        try
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id) || obj == null)
            {
                _logger.LogWarning("Edit POST received invalid data.");
                return BadRequest("Invalid request.");
            }

            var success = await _companyService.UpdateCompanyAsync(obj, id);
            return HandleOperationResult(success, "Error updating company.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating company");
            return StatusCode(500, "Internal server error.");
        }
    }
}
