

namespace JourneyMate.Application.DTOs
{
    public class CompanyDetailsDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? Slogan { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateOnly EstablishedDate { get; set; }
        public double Rating { get; set; }
        public ICollection<SocialMediaLinkDto> SocialMediaLinks { get; set; }
        public ICollection<PaymentMethodDto> PaymentMethods { get; set; }
        public ICollection<CompanyRatingDto> Ratings { get; set; }
        public ICollection<TravelDetailsDto> Travels { get; set; }
        public List<WorkingHourDto> WorkingHours { get; set; } = new List<WorkingHourDto>();
    }
}
