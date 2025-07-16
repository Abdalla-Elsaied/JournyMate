using System;

namespace JourneyMate.Application.DTOs.Recommendation;

public class InteractionDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
