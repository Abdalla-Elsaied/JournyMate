using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class TravelListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableSeats { get; set; }
        public string DeparturePoint { get; set; }
        public string DestinationCity { get; set; }
        public string TransportationType { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } 
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}