namespace JourneyMate.Application.ServicesApi.Interfaces;

public interface ITourismCompanyRequestService
{
    Task<bool> CreateTourismCompanyRequestAsync(TourismCompanyRequestDto tourismCompanyRequestVm);
    Task<bool> DeleteTourismCompanyRequestAsync(int id);
    Task<List<TourismCompanyRequestDto>> GetAllTourismCompanyRequestAsync();
    Task<bool> UpdateTourismCompanyRequestAsync(int id);

}
