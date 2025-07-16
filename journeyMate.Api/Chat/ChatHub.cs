using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text;
using JourneyMate.Application.DTOs.ChatBot;
using JourneyMate.Application.IServeces;
using JourneyMate.Application.ViewModel;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Core.Models;
using JourneyMate.Application.DTOs.ChatBot.Enum;
using JourneyMate.Application.DTOs;
using Org.BouncyCastle.Crypto;

namespace journeyMate.Api.Chat;

public class ChatHub : Hub
{

    private const string key = "your-super-secret-api-key-123";
    private const string url = "https://ahmedtarek1-ai.hf.space/chatbot/chatbot/";
    private readonly ITravelServices _travelService;
    private readonly IIpiEnventService _ipiEnventService;
    private readonly HttpClient _httpClient;

    public ChatHub(ITravelServices travelService, IIpiEnventService ipiEnventService, HttpClient httpClient)
    {
        _travelService = travelService;
        _ipiEnventService = ipiEnventService;
        _httpClient = httpClient;
    }

    public async Task SendMessage(string message)
    {
        var aiResponse = await SendMessageAsync(message);
        if (aiResponse == null)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Error processing your request.");
            return;
        }
        if (aiResponse.Recommendations is not null && aiResponse.Type == RecommendationType.Event)
        {
            var events = await GetEvents(aiResponse.Recommendations);
            var responseMessage = new ApiChatResponse
            {
                Response = aiResponse.Response,
                Events = events
            };
            await Clients.Caller.SendAsync("ReceiveMessage", responseMessage);

        }
        else if (aiResponse.Recommendations is not null && aiResponse.Type == RecommendationType.Travel)
        {
            List<TravelDetailsDto>? travels = await GetTravels(aiResponse.Recommendations);
            var responseMessage = new ApiChatResponse
            {
                Response = aiResponse.Response,
                Travels = travels
            };
            await Clients.Caller.SendAsync("ReceiveMessage", responseMessage);

        }
        else
        {
            var responseMessage = new ApiChatResponse
            {
                Response = aiResponse.Response,

            };
            await Clients.Caller.SendAsync("ReceiveMessage", responseMessage);
            return;
        }


    }
    private async Task<AiChatResponse?> SendMessageAsync(string message)
    {
        try
        {


            var request = new HttpRequestMessage(HttpMethod.Post, url);

            // Add API Key
            request.Headers.Add("AI_API_Key", key);

            // Add Bearer Token
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "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");

            var connectionId = Context.ConnectionId;

            var payload = new AiChatRequest
            {
                Message = message,
                SessionId = connectionId
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error ({response.StatusCode}): {error}");
            }
            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                AiChatResponse? events = JsonSerializer.Deserialize<AiChatResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return events;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

        }
        return null;
    }

    private async Task<List<TravelDetailsDto>> GetTravels(List<string> travelIds)
    {
        var travelItems = new List<TravelDetailsDto>();

        foreach (var idStr in travelIds)
        {
            if (!int.TryParse(idStr, out var id))
                continue;

            var travel = await _travelService.GetTravelById(id);
            if (travel is not null)
                travelItems.Add(travel);
        }

        return travelItems;
    }

    private async Task<List<APIEventItem>> GetEvents(List<string> eventlIds)
    {
        var travelItems = new List<APIEventItem>();
        foreach (var eventlId in eventlIds)
        {
            if (!int.TryParse(eventlId, out var id))
                continue;
            APIEventItem? travelItem = await _ipiEnventService.GetIpiEventByIdAsync(id);
            if (travelItem is not null)
                travelItems.Add(travelItem);

        }
        return travelItems;
    }

}