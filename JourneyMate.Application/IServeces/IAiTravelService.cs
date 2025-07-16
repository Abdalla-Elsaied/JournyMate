using System;
using JourneyMate.Application.DTOs.Recommendation;

namespace JourneyMate.Application.IServeces;

public interface IAiTravelService
{
    Task<bool> SyncTravelAsync(Travel travel);
    Task<bool> UpdateTravelAsync(Travel travel);
    Task<bool> DeleteTravelFromAIServiceAsync(string travelId);
}
