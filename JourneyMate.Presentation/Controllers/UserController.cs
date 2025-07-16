using Microsoft.AspNetCore.Authorization;

namespace JourneyMate.Presentation.Controllers;

[Authorize(Roles = "Super,Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index(string email)
    {
        var usersViewModel = await _userService.GetAllUsersAsync(email);
        return View(usersViewModel);
    }

    public async Task<IActionResult> Details(string? id, string ViewName = "Details")
    {
        if (id is null) return BadRequest();

        UserVm? userVm = await _userService.GetUserDetailsAsync(id);
        if (userVm == null) return NotFound();

        var isCompany = userVm.Roles.Contains("Company");
        if (isCompany && ViewName == "Details")
        {
            var companyDetails = await _userService.GetCompanyDetailsAsync(id);
            return View("CompanyDetails", companyDetails);
        }

        return View(ViewName, userVm);
    }

    public async Task<IActionResult> Edit(string? id) => await Details(id, "Edit");

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] string id, UserVm userVm)
    {
        if (id != userVm.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            var result = await _userService.UpdateUserAsync(id, userVm);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(userVm);
    }

    public async Task<IActionResult> Delete(string? id) => await Details(id, "Delete");

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] string? id, UserVm deletedUser)
    {
        if (ModelState.IsValid)
        {
            if (id != deletedUser.Id) return BadRequest();

            var result = await _userService.DeleteUserAsync(id);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
        }
        

        return View(deletedUser);
    }
}
