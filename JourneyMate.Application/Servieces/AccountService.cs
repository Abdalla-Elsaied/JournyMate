using JourneyMate.Application.Setting;
using JourneyMate.Core.Interfaces;




namespace JourneyMate.Application.Servieces;

public class AccountService : IAccountService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly EmailSetting _emailSetting;

    public AccountService(RoleManager<IdentityRole> roleManager
        , UserManager<AppUser> userManger
        , SignInManager<AppUser> signInManager
        , IUnitOfWork unitOfWork,
EmailSetting emailSetting
)
    {
        _roleManager = roleManager;
        _userManager = userManger;
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _emailSetting = emailSetting;
    }

    public async Task<IEnumerable<IdentityRole>> GetReles()
    {
        List<IdentityRole>? roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }
    public async Task<bool> IsExistingUserByName(string? name)
    {
        var user = await _userManager.FindByNameAsync(name ?? "");
        if (user is not null) { return true; }
        return false;
    }
    public async Task<AppUser?> IsExistingUserByEmail(string? email)
    {
        var user = await _userManager.FindByEmailAsync(email ?? "");
        if (user is not null) { return user; }
        return null;
    }
    public async Task<bool> SignUp(SignUpVm model)
    {
        var user = new AppUser()
        {
            FullName = model.UserName,
            UserName = model.UserName,
            Email = model.Email,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, model.roleName);

            if (model.roleName == "Company")
            {
                var company = new Company
                {
                    CompanyName = model.UserName,
                    UserId = user.Id, // Link to AspNetUsers
                };
                _unitOfWork.Repository<Company>().Add(company);
                _unitOfWork.Complete();

            }
            // store cookies
            await _signInManager.SignInAsync(user, model.IsAgree);
            return true;
        }
        return false;

    }
    public async Task<string?> GetUserRoleAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
        return null;
    }
    public async Task<bool> LogInAsync(LogInVM model)
    {
        AppUser? user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var flag = await _userManager.CheckPasswordAsync(user, model.Password);
            if (flag)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return true;
                }
            }
        }
        return false; // Invalid login
    }
    public async Task<bool> ChangePasswordAsync(ChangePasswordVM model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                user.IsFirstLogin = true;
                await _userManager.UpdateAsync(user);
                return true;
            }
        }
        return false;
    }
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

//######################
    public async Task<string> GeneratePasswordResetTokenAsync(AppUser appUser)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        return token;
    }
    public async Task<bool> SendResetPasswordUrlAsync(ForgetVm model, string resetPasswordUrl)
    {
        var email = new Email
        {
            Subject = "Reset Your Password",
            To = model.Email,
            Body = resetPasswordUrl
        };
        _emailSetting.SendEmail(email);
        return true;

    }
    public async Task<bool> ResetPasswordAsync(ResetPasswordVm model, string? email, string? token)
    {
        var user = await _userManager.FindByEmailAsync(email ?? "");
        if (user != null)
        {
            var result = await _userManager.ResetPasswordAsync(user, token ?? "", model.NewPassword);
            return result.Succeeded;
        }
        return false;
    }
}
