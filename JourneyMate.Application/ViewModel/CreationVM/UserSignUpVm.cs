namespace JourneyMate.Application.ViewModel.CreationVM;

public class UserSignUpVm
{

    [Required]
    public string? UserName { get; set; }


    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
