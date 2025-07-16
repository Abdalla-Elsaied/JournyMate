using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class CompanyRatingDto
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public int CompanyId { get; set; }
        public int Rating { get; set; } // from 1 to 5
        public string? Message { get; set; } = " ";
    }
}
