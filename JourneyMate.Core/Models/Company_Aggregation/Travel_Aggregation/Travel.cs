using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;

namespace JourneyMate.Core.Models.Company_Aggregation
{
    public class Travel : ModelBase
    {
        public string? Title { get; set; } // Travel Title
        public string? Description { get; set; } // Description of the travel
        public decimal BaseCost { get; set; } 
        public decimal Price { get; set; } // Price of the travel
        public decimal  SaleDiscount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int AvailableSeats { get; set; }
        public string? DeparturePoint { get; set; } // Address or name of the departure point
        public double DeparturePointLat { get; set; } // Latitude of the departure point
        public double DeparturePointLng { get; set; } // Longitude of the departure point
        public string? DestinationCity { get; set; } // Name or address of the destination city
        public double DestinationCityLat { get; set; } // Latitude of the destination city
        public double DestinationCityLng { get; set; } // Longitude of the destination city
        public string? TransportationType { get; set; } // Like bus, plane, boat
        public List<string> Amenities { get; set; } = new List<string>();
        public string? CoverImageUrl { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<string> Included { get; set; }= new List<string>();
        public List<string> NotIncluded { get; set; }= new List<string>();
        public string? SpecialOffer { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<ItinraryDay> itineraries { get; set; } = new List<ItinraryDay>();
        public ICollection<FavoriteTravel> FavoritedByUsers { get; set; } = new List<FavoriteTravel>();

    }
}