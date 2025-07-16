using JourneyMate.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


namespace JourneyMate.Application.Servieces
{
    public class GetEventsService : BackgroundService
    {
        private const string key = "eAef5ea37o6ZXEzPkMEKz3mtrJFHIdp44cRZmBCG";
        private const string firstWebSiteUrl = "https://fn6vr0tc1c.execute-api.eu-north-1.amazonaws.com/Dev/xp-egypt";
        private const string secWebSiteUrl = "https://fn6vr0tc1c.execute-api.eu-north-1.amazonaws.com/Dev/all-events";
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DeleteExpiredEventsService> _logger;
        private readonly HttpClient _httpClient;

        public GetEventsService(IServiceScopeFactory scopeFactory, ILogger<DeleteExpiredEventsService> logger, HttpClient httpClient)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _httpClient = httpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                 await using (var scope = _scopeFactory.CreateAsyncScope())
                    {
                        var eventUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        await ProcessEventsAsync(eventUnitOfWork);

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while deleting expired events.");
                }

                await Task.Delay(TimeSpan.FromDays(6), stoppingToken);
            }
        }

        private async Task ProcessEventsAsync(IUnitOfWork unitOfWork)
        {
            var firstEvents = await GetEventsAsync(firstWebSiteUrl);

            var secEvents = await GetEventsAsync(secWebSiteUrl);

            var combinedEvents = firstEvents?
              .Concat(secEvents ?? Enumerable.Empty<EventItem>())
              .ToList();
            if (combinedEvents?.Count > 0)
            {
               await  AddEventsToDatabaseAsync(unitOfWork, combinedEvents);
                _logger.LogInformation("Processed  events ");

            }
            else
            {
                _logger.LogWarning("No events found from external sources");
            }

        }
        private async Task AddEventsToDatabaseAsync(
    IUnitOfWork unitOfWork,
    List<EventItem> events)
        {
            try
            {
                var repository = unitOfWork.Repository<EventItem>();
                await repository.AddRangeAsync(events);

               unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding events to the database.");

            }
        }
        private async Task<List<EventItem>?> GetEventsAsync(string url)
        {

            var request = new HttpRequestMessage(HttpMethod.Get,
                url);

            request.Headers.Add("x-api-key", key);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<EventItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return events;
            }

            return new List<EventItem>();
        }
    }
}
