namespace JourneyMate.Core.Models.Company_Aggregation;

public class UserRating : ModelBase
{
    public int CompanyId { get; set; } // Foreign Key to Company
    public Company? Company { get; set; } // Navigation property
    public string? UserId { get; set; } // Foreign Key to User
    public AppUser? User { get; set; } // Navigation property
    public int Rating { get; set; } // Assuming ratings are integers (1-5)
    public string Message { get; set; } = " ";
    public DateTime RatedAt { get; set; } = DateTime.Now;
}