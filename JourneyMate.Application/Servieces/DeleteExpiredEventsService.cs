using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace JourneyMate.Application.Servieces
{
    public class DeleteExpiredEventsService : BackgroundService
    {
       
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DeleteExpiredEventsService> _logger;

        public DeleteExpiredEventsService(IServiceScopeFactory scopeFactory, ILogger<DeleteExpiredEventsService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await using var scope = _scopeFactory.CreateAsyncScope();
                    
                        var eventUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var allEvents = await eventUnitOfWork.Repository<EventItem>().GetAllAsync();
                        var aPIEventItems = await eventUnitOfWork.Repository<APIEventItem>().GetAllAsync();


                        if (allEvents != null)
                        {
                            var today = DateTime.Today;
                            var expiredEvents = allEvents
                                .Where(e =>
                                {
                                    DateTime eventDate;
                                    return DateTime.TryParse(e.Date ?? e.Dates, out eventDate) && eventDate < today;
                                }).ToList();

                            foreach (var ev in expiredEvents)
                            {
                                // تأكد إن عندك الطريقة المناسبة لمسح الأحداث من الـ DB
                             eventUnitOfWork.Repository<EventItem>().Delete(ev); 
                            }

                            _logger.LogInformation($"Deleted {expiredEvents.Count} expired events.");
                        }
                        if (aPIEventItems != null)
                        {
                            var today = DateTime.Today;
                            var expiredEvents = aPIEventItems
                                .Where(e =>
                                {
                                    DateTime eventDate;
                                    return DateTime.TryParse(e.Date ?? e.Dates, out eventDate) && eventDate < today;
                                }).ToList();

                            foreach (var ev in expiredEvents)
                            {
                                // تأكد إن عندك الطريقة المناسبة لمسح الأحداث من الـ DB
                                eventUnitOfWork.Repository<APIEventItem>().Delete(ev);
                            }

                            _logger.LogInformation($"Deleted {expiredEvents.Count} expired events.");
                        }
                        eventUnitOfWork.Complete();
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while deleting expired events.");
                }

                // انتظر 24 ساعة
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
        }
}
