using JourneyMate.Application.DTOs.Recommendation;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using static JourneyMate.Application.Servieces.AiTravelService;

namespace JourneyMate.Application.Servieces;



public class AiTravelService : IAiTravelService
{

    private readonly HttpClient _httpClient;
    private readonly AiApiSettings _settings;
    private readonly ILogger<AiTravelService> _logger;

    public AiTravelService(HttpClient httpClient, IOptions<AiApiSettings> settings, ILogger<AiTravelService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        if (!_httpClient.DefaultRequestHeaders.Contains("AI_API_Key"))
            _httpClient.DefaultRequestHeaders.Add("AI_API_Key", _settings.ApiKey);

      
        _logger = logger;
    }

    public async Task<bool> SyncTravelAsync(Travel travel)
    {
        try
        {
            var aiDto = new AITravelDto
            {
                Id = $"{travel.Id}",
                Location = travel.DestinationCity ?? "Unknown",
                Date = travel.StartDate.ToString("dd/MM"), // Better for AI
                Description = travel.Description ?? "",
                Itineraries = travel.itineraries.Select(i => new AiItineraryDto
                {
                    DayNumber = i.DayNumber,
                    Activities = i.Activities ?? new List<string>()
                }).ToList()
            };

            var json = System.Text.Json.JsonSerializer.Serialize(aiDto, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation($"Sending JSON: {json}");

            var requestUrl = new Uri(new Uri(_settings.BaseUrl), "/travels/add/");
            var response = await _httpClient.PostAsJsonAsync(requestUrl, new List<AITravelDto> { aiDto });


            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"AI sync failed for Travel ID {travel.Id}. Status: {response.StatusCode}");
                return false;
            }
            return true;


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception during AI sync for Travel ID {travel.Id}");
            return false;
        }
    }
    public async Task<bool> UpdateTravelAsync(Travel travel)

    {
        try
        {
            var aiDto = new AITravelDto
            {
                Id = $"{travel.Id}",
                Location = travel.DestinationCity ?? "Unknown",
                Date = travel.StartDate.ToString("dd/MM"), // Format date for AI
                Description = travel.Description ?? "",
                Itineraries = travel.itineraries.Select(i => new AiItineraryDto
                {
                    DayNumber = i.DayNumber,
                    Activities = i.Activities ?? new List<string>()
                }).ToList()
            };

            var requestUrl = new Uri(new Uri(_settings.BaseUrl), "/travels/update/");
            var response = await _httpClient.PutAsJsonAsync(requestUrl, new List<AITravelDto> { aiDto });

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"AI sync update failed for Travel ID {travel.Id}. Status: {response.StatusCode}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception during AI sync update for Travel ID {travel.Id}");
            return false;
        }
    }


    public async Task<bool> DeleteTravelFromAIServiceAsync(string travelId)
    {
        try
        {
            var requestUrl = new Uri(new Uri(_settings.BaseUrl), "/travels/delete/");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = requestUrl,
                Content = JsonContent.Create(new[] { travelId }) // Or just travelId, depending on your API
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"AI sync delete failed for Travel ID {travelId}. Status: {response.StatusCode}");
                return false;
            }

            _logger.LogInformation($"Successfully deleted Travel ID {travelId} from AI service.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception during AI sync delete for Travel ID {travelId}");
            return false;
        }
    }
}


