using System;

namespace JourneyMate.Application.DTOs.Recommendation;

public class AITravelDto
{
    public string Id { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<AiItineraryDto> Itineraries { get; set; } = new();

}
