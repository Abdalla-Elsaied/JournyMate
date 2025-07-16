

namespace JourneyMate.Application.IServeces;

public interface IAiEventService
{
    Task<bool> SyncEventsAsync(APIEventItem eventItem);

}
