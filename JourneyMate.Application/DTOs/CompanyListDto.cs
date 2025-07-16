
namespace JourneyMate.Application.DTOs
{
    public  class CompanyListDto
    {
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
    }

}

