using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.IServeces
{
    public interface IEventItemServece
    {
        Task<EventsViewModel?> GetEventsAsync(
            string searchTerm = "",
            string category = "",
            string sortBy = "date",
            string sortOrder = "asc",
            int page = 1,
            int pageSize = 9);

        Task<List<EventItem>?> GetAllEventsAsync();
        Task<bool> ConfirmedEventAsync(int id);
        Task<bool> DeleteEventAsync(int id);
        Task<List<string>> GetCategoriesAsync();
        Task<int> GetTotalEventsCountAsync();
    }

    public class EventsViewModel
    {
        public List<EventItem> Events { get; set; } = new List<EventItem>();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalEvents { get; set; } = 0;
        public int TotalPages => (int)Math.Ceiling((double)TotalEvents / PageSize);
        public string SearchTerm { get; set; } = "";
        public string Category { get; set; } = "";
        public string SortBy { get; set; } = "date";
        public string SortOrder { get; set; } = "asc";
        public List<string> Categories { get; set; } = new List<string>();

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int PreviousPage => CurrentPage - 1;
        public int NextPage => CurrentPage + 1;

        public List<int> GetPageNumbers()
        {
            var pages = new List<int>();
            var start = Math.Max(1, CurrentPage - 2);
            var end = Math.Min(TotalPages, CurrentPage + 2);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            return pages;
        }
    }
}