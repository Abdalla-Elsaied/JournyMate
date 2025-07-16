using System;
using System.Net.Http.Json;
using System.Text.Json;
using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Application.ServicesApi.Interfaces;
using Microsoft.Extensions.Options;

namespace JourneyMate.Application.ServicesApi.Implementaion;

public class RecommendationServices : IRecommendationServices
{
    private readonly HttpClient _httpClient;
    private readonly AiApiSettings _settings;
    private readonly IUnitOfWork _unitOfWrok;
    private readonly ILogger<RecommendationServices> logger;
    private readonly IMapper _mapper;
    public RecommendationServices(HttpClient httpClient, IOptions<AiApiSettings> settings, IUnitOfWork unitOfWrok, IMapper mapper, ILogger<RecommendationServices> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        if (!_httpClient.DefaultRequestHeaders.Contains("AI_API_Key"))
            _httpClient.DefaultRequestHeaders.Add("AI_API_Key", _settings.ApiKey);

     

        _unitOfWrok = unitOfWrok;
        _mapper = mapper;
        this.logger = logger;
    }
    public async Task<bool> SetInteractionsAsync(UserInteractionDto interactions)
    { 
        var list =  new List<UserInteractionDto>()
        {
            interactions
        };
        var requestUrl = new Uri(new Uri(_settings.BaseUrl), "/users/add/");
           var json = System.Text.Json.JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            logger.LogInformation($"Sending JSON: {json}");

        var response = await _httpClient.PostAsJsonAsync(requestUrl, list);
        if (!response.IsSuccessStatusCode)
        {
            
            logger.LogWarning($"AI sync failed after add User interaction for the first time");
            return false;
        }

        return true;
       
    }
    public async Task<bool> UpdateInteractionsAsync(UserInteractionDto interactions)
    { 
        var list =  new List<UserInteractionDto>()
        {
            interactions
        };
        var requestUrl = new Uri(new Uri(_settings.BaseUrl), "/users/update/");
        
        var response = await _httpClient.PutAsJsonAsync(requestUrl, list);
        if (!response.IsSuccessStatusCode)
        {
            
            logger.LogWarning($"AI sync failed after add User interaction for the first time");
            return false;
        }

        return true;
       
    }

    public async Task<IReadOnlyList<TravelDetailsDto>> GetRecommendedTravelsAsync(string userId, int? numRecommendations = null, int? numHighestInteractions = null)
    {
        var uriBuilder = new UriBuilder(new Uri(new Uri(_settings.BaseUrl), $"/users/recommend_travels/{userId}"));

        var queryParams = new List<string>();
        if (numRecommendations.HasValue)
            queryParams.Add($"Num_recommendations={numRecommendations.Value}");
        if (numHighestInteractions.HasValue)
            queryParams.Add($"Num_highest_intercations={numHighestInteractions.Value}");

        if (queryParams.Any())
            uriBuilder.Query = string.Join("&", queryParams);

        var response = await _httpClient.GetAsync(uriBuilder.Uri);
        if (!response.IsSuccessStatusCode)
            return new List<TravelDetailsDto>();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("recommendations", out var recommendationsElement) || recommendationsElement.ValueKind != JsonValueKind.Array)
            return new List<TravelDetailsDto>();

        var travelIds = new List<int>();

        foreach (var item in recommendationsElement.EnumerateArray())
        {
            var str = item.GetString();
            if (string.IsNullOrWhiteSpace(str)) continue;

            // Extract number from string like "travel15"
            var digits = new string(str.Where(char.IsDigit).ToArray());
            if (int.TryParse(digits, out var id))
            {
                travelIds.Add(id);
            }
        }

        if (!travelIds.Any())
            return new List<TravelDetailsDto>();

        var travels = await _unitOfWrok.Repository<Travel>().GetByIdsTravelAsync(travelIds);
        return _mapper.Map<IReadOnlyList<TravelDetailsDto>>(travels);
    }

    public async Task<IReadOnlyList<EventDto>> GetRecommendedEventsAsync(
        string userId, int? numRecommendations = null, int? numHighestInteractions = null)
    {
        var uriBuilder = new UriBuilder(new Uri(new Uri(_settings.BaseUrl), $"/users/recommend_events/{userId}"));

        var queryParams = new List<string>();
        if (numRecommendations.HasValue)
            queryParams.Add($"Num_recommendations={numRecommendations.Value}");
        if (numHighestInteractions.HasValue)
            queryParams.Add($"Num_highest_interactions={numHighestInteractions.Value}");

        if (queryParams.Any())
            uriBuilder.Query = string.Join("&", queryParams);

        var response = await _httpClient.GetAsync(uriBuilder.Uri);
        if (!response.IsSuccessStatusCode)
            return new List<EventDto>();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("recommendations", out var recommendationsElement) ||
            recommendationsElement.ValueKind != JsonValueKind.Array)
        {
            return new List<EventDto>();
        }

        var eventIds = new List<int>();

        foreach (var item in recommendationsElement.EnumerateArray())
        {
            var str = item.GetString();
            if (string.IsNullOrWhiteSpace(str)) continue;

            if (int.TryParse(str, out var id))
            {
                eventIds.Add(id);
            }
        }

        if (!eventIds.Any())
            return new List<EventDto>();

        var events = await _unitOfWrok.Repository<APIEventItem>().GetByIdsEventAsync(eventIds);

        return _mapper.Map<IReadOnlyList<EventDto>>(events);
    }

}
