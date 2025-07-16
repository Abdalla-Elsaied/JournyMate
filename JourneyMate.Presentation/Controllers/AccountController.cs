using JourneyMate.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace JourneyMate.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    #region Sign Up
      [Authorize(Roles = "Super,Admin")]
    public async Task<IActionResult> SignUp()
    {
        var roles =  (await _accountService.GetReles()).Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToList();

        ViewData["Roles"] = roles;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpVm model)
    {
        if (ModelState.IsValid) //server side validation
        {
            var isExisted = await _accountService.IsExistingUserByName(model.UserName);
            if (isExisted)
                ModelState.AddModelError(string.Empty, "Username is already exists");

            var isCreated = await _accountService.SignUp(model);
            if (isCreated)
            {
                return RedirectToAction("Index", "User");
            }
        }
        var roles = (await _accountService.GetReles()).Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToList();

        ViewData["Roles"] = roles;
        return View();
    }
    #endregion

    #region Login
    public IActionResult LogIn()
    {
        return View("Login");

    }
    [HttpPost]
    [AllowAnonymous] // Allow anonymous access to the Login action
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> LogIn(LogInVM model)
    {
        if (ModelState.IsValid)
        {
            var success = await _accountService.LogInAsync(model);
            if (success)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var role = await _accountService.GetUserRoleAsync(userId ?? "");
                if (role == "Company")
                {
                    if (!await IsFirstLogin(model.Email)) // Helper method to check IsFirstLogin
                    {
                        return View("ChangePassword");
                    }
                    HttpContext.Session.SetString("CurrentCompany", userId);
                    return RedirectToAction("Index", "CompanyDash");
                }
                return RedirectToAction("Index", "User");
            }
            ModelState.AddModelError(string.Empty, "Invalid login credentials.");
        }
        return View("Login", model);
    }
    private async Task<bool> IsFirstLogin(string email)
    {
        // This would typically use IAccountService or UserManager to check IsFirstLogin
        AppUser? user = await _accountService.IsExistingUserByEmail(email ?? ""); // Assume this method exists in IAccountService
        return user?.IsFirstLogin ?? true;
    }
    #endregion

    #region Change Password
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("LogIn");
        }

        var success = await _accountService.ChangePasswordAsync(model, userId);
        if (success)
        {
            HttpContext.Session.SetString("CurrentCompany", userId);
            return RedirectToAction("Index", "CompanyDash");
        }

        ModelState.AddModelError(string.Empty, "Password change failed. Please check your input.");
        return View(model);
    }
    #endregion

    #region SignOut
    public async Task<IActionResult> SignOut()
    {
        await _accountService.SignOutAsync();
        return RedirectToAction("LogIn");
    }
    #endregion

    #region Forget Password
    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendResetPasswordUrl(ForgetVm model)
    {
        if (ModelState.IsValid)
        {
            var user = await _accountService.IsExistingUserByEmail(model.Email);
            var token = await _accountService.GeneratePasswordResetTokenAsync(user);
            var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
            var success = await _accountService.SendResetPasswordUrlAsync(model, resetPasswordUrl ??"");
            if (success)
            {
                return RedirectToAction(nameof(CheckYourInput));
            }
            ModelState.AddModelError(string.Empty, "Invalid email address.");
        }
        return View(model);
    }

    public IActionResult CheckYourInput()
    {
        return View();
    }
    #endregion

    #region Reset Password
    public IActionResult ResetPassword(string email, string token)
    {
        TempData["email"] = email;
        TempData["token"] = token;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVm model)
    {
        if (ModelState.IsValid)
        {
            string? email = TempData["email"] as string;
            string? token = TempData["token"] as string;
            var success = await _accountService.ResetPasswordAsync(model, email, token);
            if (success)
            {
                return RedirectToAction(nameof(LogIn));
            }
            ModelState.AddModelError(string.Empty, "Password reset failed. Please try again.");
        }
        return View(model);
    }
    #endregion
}