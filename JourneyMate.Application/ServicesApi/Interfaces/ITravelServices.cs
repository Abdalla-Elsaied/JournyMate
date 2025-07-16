using JourneyMate.Application.Common;
using JourneyMate.Application.DTOs;
using JourneyMate.Infrastracture.Specefications.TravelSpecifications;

namespace JourneyMate.Application.ServicesApi.Interfaces
{
    public interface ITravelServices
    {
        Task<IReadOnlyList<TravelDetailsDto>> GetAllTravles(TravelSpecPrams travelSpecPrams);
    
        Task<TravelDetailsDto?> GetTravelById(int id);
        Task<IReadOnlyList<TravelDetailsDto>> GetDiscountedTravles(TravelSpecPrams travelPrams);
        Task<IReadOnlyList<TravelDetailsDto>> GetTravelsLeavingSoon(TravelSpecPrams travelPrams);
        Task<int> GetCountAsync(TravelSpecPrams travelSpec, ISpec<Travel> spec);
        Task<ServiceResult> AddLikeToTravel(int travelId, string userId);
        Task<ServiceResult> RemoveLikeFromTravel(int travelId, string userId);
        Task<List<TravelDetailsDto>> GetFavTravelsByUserId (string userId);
        Task<IReadOnlyList<TravelDetailsDto>> GetNewestTravelsForCompany(TravelSpecPrams travelPrams, int companyId);
        Task<IReadOnlyList<TravelDetailsDto>> GetNewestTravels(TravelSpecPrams travelPrams);

    }
}