using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models.Company_Aggregation
{
    public class WorkingHour
    {

        public string DayOfWeek { get; set; }  // Monday, Tuesday, etc.

        public TimeSpan? OpenTime { get; set; }  // Example: 09:00
        public TimeSpan? CloseTime { get; set; } // Example: 23:00
        
        public bool IsClosed => OpenTime == null || CloseTime == null;
    }
}
