using JourneyMate.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs;

public class TourismCompanyRequestDto
{
    public int Id { get; set; }

    [Required]
    public string? CompanyName { get; set; }
    [Required]

    public string? Owner { get; set; }
    [Required]
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
