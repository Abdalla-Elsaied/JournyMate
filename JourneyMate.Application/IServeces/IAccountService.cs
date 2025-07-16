namespace JourneyMate.Application.IServeces;

public interface IAccountService
{
    Task<IEnumerable<IdentityRole>> GetReles();
    Task<bool> SignUp(SignUpVm model);
    Task<bool> IsExistingUserByName(string? name);
    Task<AppUser?> IsExistingUserByEmail(string? email);
    Task<string?> GetUserRoleAsync(string userId);
    Task<bool> LogInAsync(LogInVM model);
    Task<bool> ChangePasswordAsync(ChangePasswordVM model, string userId);
    Task SignOutAsync();
    Task<bool> SendResetPasswordUrlAsync(ForgetVm model, string resetPasswordUrl);
    Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
    Task<bool> ResetPasswordAsync(ResetPasswordVm model, string? email, string? token);
}
