using System;

namespace JourneyMate.Application.DTOs.Recommendation;

public class AiItineraryDto
{
    public int DayNumber { get; set; }
    public List<string> Activities { get; set; } = new();

}
