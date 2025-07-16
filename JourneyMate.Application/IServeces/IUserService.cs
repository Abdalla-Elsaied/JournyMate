namespace JourneyMate.Application.IServeces;

public interface IUserService
{
    Task<UsersViewModel> GetAllUsersAsync(string? email);
    Task<UserVm?> GetUserDetailsAsync(string id);
    Task<CompanyDetailsVM?> GetCompanyDetailsAsync(string id);
    Task<IdentityResult> UpdateUserAsync(string id, UserVm userVm);
    Task<IdentityResult> DeleteUserAsync(string id);
}