using JourneyMate.Core.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;


namespace JourneyMate.Core.Models;

public class TourismCompanyRequest : ModelBase
{
    [Required]
    public string? CompanyName { get; set; }
    [Required]
    public string? Owner { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? CommercialRegistrationNumber { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }
    public string? WebsiteUrl { get; set; }
    [Required]
    public string? CompanyAddress { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? ContactPersonName { get; set; }
    [Required]
    public string? ContactPersonNumber { get; set; }
    [Required]
    public string? TypeofTrips { get; set; }
    public Status Status { get; set; } = Status.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? LicenseImageUrl { get; set; }


}
