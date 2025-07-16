using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JourneyMate.Core.Models.Company_Aggregation;

namespace JourneyMate.Core.Models
{
    public class Image : ModelBase
    {
        public string ImageUrl { get; set; } = string.Empty;
        public Travel? Travel { get; set; }
        [ForeignKey("Travel")]
        public int TravelId { get; set; }
    }
}
