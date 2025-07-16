using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models.Company_Aggregation
{
    public class ItinraryDay:ModelBase
    {      
        public Travel? Travel { get; set; } 
        public int TravelId { get; set; }
    
        public string Title { get; set; }

        public int DayNumber { get; set; }

    
        public string Description { get; set; }

      
        public TimeSpan? StartTime { get; set; }

      
        public TimeSpan? EndTime { get; set; }

      
        public string Location { get; set; }

       
        public List<string> Activities { get; set; } = new List<string>();

       
        public bool IncludesBreakfast { get; set; }

      
        public bool IncludesLunch { get; set; }

      
        public bool IncludesDinner { get; set; }

      
        public string Notes { get; set; }

    }
}
