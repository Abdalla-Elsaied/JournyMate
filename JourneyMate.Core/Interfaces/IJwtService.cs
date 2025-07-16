namespace JourneyMate.Core.Interfaces;

public interface IJwtService
{
    string GenerateToken(string userId, string role);
    Task<string> CreatTokenAsync(AppUser appUser, UserManager<AppUser> userManager);
}
