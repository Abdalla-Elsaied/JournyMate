using System;

namespace JourneyMate.Application.DTOs.Recommendation;

public class UserInteractionDto
{
    public string Id { get; set; } = string.Empty;
    public List<InteractionDto>? UserInteraction { get; set; }
}
