using JourneyMate.Application.DTOs.ChatBot.Enum;
using System.Text.Json.Serialization;

namespace JourneyMate.Application.DTOs.ChatBot;

public class AiChatResponse
{
    [JsonPropertyName("response")]
    public string? Response { get; set; }


    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    [JsonPropertyName("recommendations")]
    public List<string>? Recommendations { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RecommendationType Type { get; set; }


}
