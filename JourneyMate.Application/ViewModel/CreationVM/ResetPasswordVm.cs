namespace JourneyMate.Application.ViewModel.CreationVM
{
    public class ResetPasswordVm
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(NewPassword), ErrorMessage = "Confirm Password doesn't match Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
