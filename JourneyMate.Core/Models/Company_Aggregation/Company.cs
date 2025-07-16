namespace JourneyMate.Core.Models.Company_Aggregation;

public class Company : ModelBase
{
    public string? CompanyName { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; } 
    public string? Description { get; set; }
    public string? Slogan { get; set; }
    public string? Website { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateOnly EstablishedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public float Rating { get; set; } = 0;
    public ICollection<UserRating> Ratings { get; set; } = new List<UserRating>();
    public List<Travel> Travels { get; set; } = new List<Travel>();
    public List<WorkingHour> WorkingHours { get; set; } = new List<WorkingHour>();
    public ICollection<SocialMediaLink> SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
    public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}
