namespace JourneyMate.Application.ViewModel;

public class CompanyDetailsVM

{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string phoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<Travel> Travels { get; set; } = new List<Travel>();
}
