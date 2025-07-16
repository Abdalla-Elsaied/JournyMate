using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
namespace JourneyMate.Application.ViewModel;

public class TravelVm 
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }
  

    [Required(ErrorMessage = "Price is required.")]

    [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
    public decimal BaseCost { get; set; }

    [Range(0, 100, ErrorMessage = "Price must be non-negative")]
    public decimal SaleDiscount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Start Date is required.")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End Date is required.")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreationDate { get; set; }

    [Required(ErrorMessage = "Available Seats is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Available seats must be non-negative")]
    public int AvailableSeats { get; set; }

    [Required(ErrorMessage = "Departure Point is required.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Departure Point must be between 1 and 255 characters.")]
    public string? DeparturePoint { get; set; }

    public double? DeparturePointLat { get; set; }
    public double? DeparturePointLng { get; set; }

    [Required(ErrorMessage = "Destination City is required.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Destination City must be between 1 and 255 characters.")]
    public string? DestinationCity { get; set; }

    public double? DestinationCityLat { get; set; }
    public double? DestinationCityLng { get; set; }

    [Required(ErrorMessage = "Transportation Type is required.")]
    public string? TransportationType { get; set; }
    public List<string> Amenities { get; set; } = new List<string>();
    public List<string> Included { get; set; } = new List<string>();
    public List<string> NotIncluded { get; set; } = new List<string>();
    public IFormFile? IFormFileCoverImage { get; set; }
    public string? CoverImageUrl { get; set; }
    public IFormFile? IFormFileProfileImage { get; set; }
    public string? ProfileImageUrl { get; set; }

    [Required(ErrorMessage = "Company Id is required.")]
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public List<IFormFile> IFormFileImages { get; set; } = new List<IFormFile>(); // Assuming you want to display images
    public List<ImageVM> Images { get; set; } = new List<ImageVM>();
    public ICollection<ItineraryDayVm> itenraries { get; set; } = new List<ItineraryDayVm>();
    [Required(ErrorMessage = "Category is required.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; } 
    public IEnumerable<SelectListItem>? Categories { get; set; } 
    public string? CategoryName { get; set; }
}
public class ImageVM : ModelBase
{
    public string ImageUrl { get; set; } = string.Empty;
}

public class ItineraryDayVm
{
    public int Id { get; set; }
    public int TravelId { get; set; }

    [Required]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Day Number")]
    public int DayNumber { get; set; }

    [Required]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Start Time")]
    public TimeSpan? StartTime { get; set; }

    [Display(Name = "End Time")]
    public TimeSpan? EndTime { get; set; }

    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;

    [Display(Name = "Activities")]
    public List<string> Activities { get; set; } = new List<string>();

    [Display(Name = "Includes Breakfast")]
    public bool IncludesBreakfast { get; set; }

    [Display(Name = "Includes Lunch")]
    public bool IncludesLunch { get; set; }

    [Display(Name = "Includes Dinner")]
    public bool IncludesDinner { get; set; }

    [Display(Name = "Notes")]
    public string Notes { get; set; } = string.Empty;
    
}