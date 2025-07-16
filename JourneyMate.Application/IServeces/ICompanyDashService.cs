namespace JourneyMate.Application.IServeces;

public interface ICompanyDashService
{
    Task<CompanyVm?> GetCompanyByUserIdAsync(string userId);
    Task<bool> UpdateCompanyAsync(CompanyVm companyVm, string userId);
}