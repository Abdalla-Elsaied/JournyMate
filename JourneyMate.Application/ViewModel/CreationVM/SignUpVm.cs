namespace JourneyMate.Application.ViewModel.CreationVM
{
    public class SignUpVm
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string roleName { get; set; }

        public bool IsAgree { get; set; }
    }
}
