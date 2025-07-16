namespace JourneyMate.Application.ViewModel
{
    public class UserVm
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public UserVm()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
