using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models
{
    public class EventItemBase : ModelBase
    { 
    
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Dates { get; set; }
        public string? Date { get; set; }
        public string? Location { get; set; }
        public string? Link { get; set; }
        public string? Image { get; set; }
        public string? Map_Link { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }

    }
}
