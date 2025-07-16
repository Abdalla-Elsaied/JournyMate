using System;

namespace JourneyMate.Application.DTOs.Recommendation;

public class EventAiDto
{
    public string Id { get; set; } = default!;
    public string? Location { get; set; } = "no location";
    public string? Date { get; set; } = "no date";
    public string? Description { get; set; } = "no Description";

}
