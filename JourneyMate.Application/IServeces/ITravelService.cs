using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;

namespace JourneyMate.Application.IServeces;
public interface ITravelService
{
    Task<List<Travel>> GetTravelsByCompanyAsync(string userId);
    Task<TravelVm> GetTravelByIdAsync(int id);
    Task<bool> CreateTravelAsync(CreateTravelVmParamter TravelVmParamter);
    Task<bool> UpdateTravelAsync(int id, TravelVm travelVm, List<int> imagesToRemove, List<IFormFile> newImages);
    Task<bool> DeleteTravelAsync(int id);
    Task<IEnumerable<Category>?> GetCategories();
}

