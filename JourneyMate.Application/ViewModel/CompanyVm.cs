namespace JourneyMate.Application.ViewModel
{
    public class CompanyVm
    {
        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(255, ErrorMessage = "Company Name cannot exceed 255 characters.")]
        public string? CompanyName { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public string? UserId { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }
        [StringLength(255, ErrorMessage = "Slogan URL cannot exceed 255 characters.")]
        public string? Slogan { get; set; }

        [StringLength(255, ErrorMessage = "Website URL cannot exceed 255 characters.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? Website { get; set; }

        [StringLength(255, ErrorMessage = "Profile Image URL cannot exceed 255 characters.")]
        [Url(ErrorMessage = "Please enter a valid URL for the profile image.")]
        public string? ProfileImageUrl { get; set; }

        [StringLength(255, ErrorMessage = "Profile Image URL cannot exceed 255 characters.")]
        [Url(ErrorMessage = "Please enter a valid URL for the profile image.")]
        public string? CoverImageUrl { get; set; }

        [Required(ErrorMessage = "Established Date is required.")]
        [DataType(DataType.Date)]
        public DateOnly EstablishedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public double Rating { get; set; } = 0.0;
        public List<TravelVm> Travels { get; set; } = new List<TravelVm>();
        public List<SocialMediaLinkViewModel>? SocialMediaLinks { get; set; } = new List<SocialMediaLinkViewModel>();
        public List<PaymentMethodViewModel>? PaymentMethods { get; set; } = new List<PaymentMethodViewModel>();
        public List<WorkingHourViewModel> WorkingHours { get; set; } = new List<WorkingHourViewModel>();
        public IFormFile? ProfileImageFile { get; set; }
        public IFormFile? CoverImageFile { get; set; }

    }

    // Example ViewModel for related entities if needed
    public class SocialMediaLinkViewModel : ModelBase
    {
        public string? Platform { get; set; }
        public string? Url { get; set; }
    }

    public class PaymentMethodViewModel : ModelBase
    {
        public string? MethodName { get; set; }
        public string? Details { get; set; }
    }

    public class WorkingHourViewModel 
    {
        public string DayOfWeek { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed => OpenTime == null || CloseTime == null;
    }
}