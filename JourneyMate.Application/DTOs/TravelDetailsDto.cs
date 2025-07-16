using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class TravelDetailsDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal BaseCost { get; set; }
        public decimal SaleDiscount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int AvailableSeats { get; set; }
        public string? DeparturePoint { get; set; }
        public double DeparturePointLat { get; set; }
        public double DeparturePointLng { get; set; }
        public string? DestinationCity { get; set; }
        public double DestinationCityLat { get; set; }
        public double DestinationCityLng { get; set; }
        public string? TransportationType { get; set; }
        public List<string> Amenities { get; set; } = new List<string>();       
        public string? CoverImageUrl { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CompanyProfileImageUrl { get; set; }
        public List<string> Included { get; set; } = new List<string>();
        public List<string> NotIncluded { get; set; } = new List<string>();
        public string? SpecialOffer { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>(); 
        public List<ItinraryDayDto> itineraries  { get; set; } = new List<ItinraryDayDto>();          
    }
}
