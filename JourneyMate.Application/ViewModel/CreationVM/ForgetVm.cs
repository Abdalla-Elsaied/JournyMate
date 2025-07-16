namespace JourneyMate.Application.ViewModel.CreationVM
{
    public class ForgetVm
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
