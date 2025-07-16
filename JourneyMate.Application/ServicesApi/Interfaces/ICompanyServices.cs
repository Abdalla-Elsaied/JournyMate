using JourneyMate.Application.Common;
using JourneyMate.Application.DTOs;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;

namespace JourneyMate.Application.ServicesApi.Interfaces
{
    public interface ICompanyServices
    {

        Task<IEnumerable<CompanyDetailsDto>?> GetAllCompaniessBySpec(CompanySpecParams companySpecParams);
        Task<CompanyDetailsDto?> GetCompanyById(int id);
        Task<int> GetCountAsync(CompanySpecParams companySpec);
        Task<CompanyDetailsDto?> UpdateRate(CompanyDetailsDto companyDetailsDto);
        Task<List<string>> GetCompanyImages(int id);

    }
}