using System;
using JourneyMate.Application.DTOs.Recommendation;

namespace JourneyMate.Application.ServicesApi.Interfaces;

public interface IRecommendationServices
{
    Task<bool> SetInteractionsAsync(UserInteractionDto interactions);
    Task<bool> UpdateInteractionsAsync(UserInteractionDto interactions);
    Task<IReadOnlyList<TravelDetailsDto>> GetRecommendedTravelsAsync(string userId, int? numRecommendations = null, int? numHighestInteractions = null);
    Task<IReadOnlyList<EventDto>> GetRecommendedEventsAsync(string userId, int? numRecommendations = null, int? numHighestInteractions = null);
}
