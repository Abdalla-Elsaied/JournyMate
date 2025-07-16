using System;
using System.Net.Http.Json;
using System.Text.Json;
using JourneyMate.Application.DTOs.Recommendation;
using Microsoft.Extensions.Options;

namespace JourneyMate.Application.Servieces;

public class AiEventService:IAiEventService
{
    private readonly HttpClient _httpClient;
    private readonly AiApiSettings _settings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AiEventService> _logger;
    public AiEventService(
            HttpClient httpClient,
            IOptions<AiApiSettings> settings,
            IUnitOfWork unitOfWork,
            ILogger<AiEventService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _unitOfWork = unitOfWork;
        _logger = logger;

        if (!_httpClient.DefaultRequestHeaders.Contains("AI_API_Key"))
            _httpClient.DefaultRequestHeaders.Add("AI_API_Key", _settings.ApiKey);
    }
    public async Task<bool> SyncEventsAsync(APIEventItem eventItem)
    {

        var aiDto = new EventAiDto
        {
            Id = $"{eventItem.Id}",
            Location = eventItem.Location ?? "no location",
            Date = (eventItem.Date ?? eventItem.Dates) ?? "no date",
            Description = eventItem.Description ?? "no Description"
        };

        var list = new List<EventAiDto> { aiDto };

        var requestUrl = new Uri(new Uri(_settings.BaseUrl), "/events/add/");

        var json = System.Text.Json.JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        _logger.LogInformation($"Sending JSON to AI API: {json}");

        var response = await _httpClient.PostAsJsonAsync(requestUrl, list);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning($"Failed to sync event with AI API. Status: {response.StatusCode}");
            return false;
        }

        return true;
    }
}

