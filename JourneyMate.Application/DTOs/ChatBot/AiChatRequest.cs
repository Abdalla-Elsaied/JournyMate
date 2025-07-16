using System.Text.Json.Serialization;

namespace JourneyMate.Application.DTOs.ChatBot;

public class AiChatRequest
{
    [JsonPropertyName("message")]

    public string? Message { get; set; }

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }
}

