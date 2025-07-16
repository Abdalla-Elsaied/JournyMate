using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class ItinraryDayDto
    {
        public string Title { get; set; } = string.Empty;

        public int DayNumber { get; set; }

        public string Description { get; set; } = string.Empty;

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string Location { get; set; } = string.Empty;

        public List<string> Activities { get; set; } = new List<string>();

        public bool IncludesBreakfast { get; set; }

        public bool IncludesLunch { get; set; }
        public bool IncludesDinner { get; set; }

        public string Notes { get; set; } = string.Empty;
    
    }
}
