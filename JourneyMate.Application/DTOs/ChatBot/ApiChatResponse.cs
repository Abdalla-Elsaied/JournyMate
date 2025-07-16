namespace JourneyMate.Application.DTOs.ChatBot
{
    public class ApiChatResponse
    {
        public string? Response { get; set; }
        public List<APIEventItem>? Events { get; set; }
        public List<TravelDetailsDto>? Travels { get; set; }
    }
}
