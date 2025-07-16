using System.Text.Json;
using System.Linq;
using Microsoft.VisualBasic;

namespace JourneyMate.Application.Servieces
{
    public class EventItemServece : IEventItemServece
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EventItemServece> _logger;
        private readonly IMapper _mapper;
        private readonly IAiEventService _aiEventService;

        public EventItemServece(IUnitOfWork unitOfWork, ILogger<EventItemServece> logger, IMapper mapper, IAiEventService aiEventService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _aiEventService = aiEventService;
        }

        public async Task<EventsViewModel?> GetEventsAsync(
            string searchTerm = "",
            string category = "",
            string sortBy = "date",
            string sortOrder = "asc",
            int page = 1,
            int pageSize = 9)
        {
            try
            {
                var allEvents = await _unitOfWork.Repository<EventItem>().GetAllAsync();
                if (allEvents == null || !allEvents.Any())
                {
                    return new EventsViewModel
                    {
                        Events = new List<EventItem>(),
                        Categories = new List<string>()
                    };
                }

                var query = allEvents.AsQueryable();

                // Apply search filter with null checks
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(e =>
                        (!string.IsNullOrEmpty(e.Title) && e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(e.Description) && e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(e.Location) && e.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
                }

                // Apply category filter with null check
                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(e => !string.IsNullOrEmpty(e.Category) &&
                                           e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                }

                // Apply sorting with null-safe comparisons
                query = sortBy.ToLower() switch
                {
                    "title" => sortOrder == "desc"
                        ? query.OrderByDescending(e => e.Title ?? string.Empty)
                        : query.OrderBy(e => e.Title ?? string.Empty),
                    "category" => sortOrder == "desc"
                        ? query.OrderByDescending(e => e.Category ?? string.Empty)
                        : query.OrderBy(e => e.Category ?? string.Empty),
                    "location" => sortOrder == "desc"
                        ? query.OrderByDescending(e => e.Location ?? string.Empty)
                        : query.OrderBy(e => e.Location ?? string.Empty),
                    "date" => sortOrder == "desc"
                        ? query.OrderByDescending(e => ParseEventDate(e))
                        : query.OrderBy(e => ParseEventDate(e)),
                    _ => query.OrderBy(e => ParseEventDate(e))
                };

                var totalEvents = query.Count();
                var events = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var categories = await GetCategoriesAsync();

                return new EventsViewModel
                {
                    Events = events,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalEvents = totalEvents,
                    SearchTerm = searchTerm ?? string.Empty,
                    Category = category ?? string.Empty,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    Categories = categories
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching events with filters.");
                return new EventsViewModel
                {
                    Events = new List<EventItem>(),
                    Categories = new List<string>()
                };
            }
        }

        public async Task<List<EventItem>?> GetAllEventsAsync()
        {
            try
            {
                var allEvents = await _unitOfWork.Repository<EventItem>().GetAllAsync();
                return allEvents?.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all events.");
                return null;
            }
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid data received for Delete Event.");
                    return false;
                }

                var eventItem = await _unitOfWork.Repository<EventItem>().GetByIdAsync(id);
                if (eventItem == null)
                {
                    _logger.LogWarning($"Event not found for ID: {id}");
                    return false;
                }

                _unitOfWork.Repository<EventItem>().Delete(eventItem);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting Event.");
                return false;
            }
        }

        public async Task<bool> ConfirmedEventAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid data received for Confirm Event.");
                    return false;
                }

                var eventItem = await _unitOfWork.Repository<EventItem>().GetByIdAsync(id);
                if (eventItem == null)
                {
                    _logger.LogWarning($"Event not found for ID: {id}");
                    return false;
                }

                // Remove from EventItem and add to APIEventItem
                _unitOfWork.Repository<EventItem>().Delete(eventItem);
                var apiEventItem = _mapper.Map<APIEventItem>(eventItem);
                apiEventItem.Id = 0; // Reset ID for new entry
                _unitOfWork.Repository<APIEventItem>().Add(apiEventItem);
                   // Request to Ai to add the event
              
                await _unitOfWork.CompleteAsync();
                bool aiSynced = await _aiEventService.SyncEventsAsync(apiEventItem);
                if (!aiSynced)
                {
                    _logger.LogWarning($"AI sync failed after updating travel {id}");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Confirming Event.");
                return false;
            }
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                var allEvents = await _unitOfWork.Repository<EventItem>().GetAllAsync();
                return allEvents?.Where(e => !string.IsNullOrWhiteSpace(e.Category))
                    .Select(e => e.Category!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList() ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching categories.");
                return new List<string>();
            }
        }

        public async Task<int> GetTotalEventsCountAsync()
        {
            try
            {
                var allEvents = await _unitOfWork.Repository<EventItem>().GetAllAsync();
                return allEvents?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting total events count.");
                return 0;
            }
        }

        private DateTime ParseEventDate(EventItem eventItem)
        {
            // Try to parse the date from Dates field first, then Date field
            var dateString = !string.IsNullOrEmpty(eventItem.Dates) ? eventItem.Dates : eventItem.Date?.ToString();

            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out DateTime parsedDate))
            {
                return parsedDate;
            }

            // Fallback to a default date if parsing fails
            return DateTime.MinValue;
        }
    }
}