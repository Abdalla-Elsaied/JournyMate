namespace JourneyMate.Core.Models.Company_Aggregation;

public class SocialMediaLink : ModelBase
{
    public int CompanyId { get; set; } // FK
    public Company? Company { get; set; } // Navigation property
    public string? Platform { get; set; } // e.g., "Twitter"
    public string? Url { get; set; }
}