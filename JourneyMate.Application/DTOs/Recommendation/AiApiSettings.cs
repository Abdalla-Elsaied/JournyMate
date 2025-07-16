using System;

namespace JourneyMate.Application.DTOs.Recommendation;

public class AiApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Authorization { get; set; } = string.Empty;
}
