namespace JourneyMate.Application.ViewModel
{
    public class UsersViewModel
    {
        public List<UserVm> CompanyUsers { get; set; } = new List<UserVm>();
        public List<UserVm> OtherUsers { get; set; } = new List<UserVm>();

        // Additional properties for enhanced dashboard
        public int TotalUsers => CompanyUsers.Count + OtherUsers.Count;
        public int AdminUsersCount => OtherUsers.Count(u => u.Roles.Contains("Admin") || u.Roles.Contains("Super"));
        public int RegularUsersCount => OtherUsers.Count(u => !u.Roles.Contains("Admin") && !u.Roles.Contains("Super"));
        public int CompanyUsersCount => CompanyUsers.Count;

        // Statistics for dashboard cards
        public UserStatistics Statistics { get; set; } = new UserStatistics();
    }

    public class UserStatistics
    {
        public int TotalUsersGrowth { get; set; } = 12; // Percentage growth
        public int CompanyUsersGrowth { get; set; } = 8;
        public int RegularUsersGrowth { get; set; } = 15;
        public int AdminUsersGrowth { get; set; } = 0;

        // You can add more statistics as needed
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public int ActiveUsersToday { get; set; }
        public int NewUsersThisWeek { get; set; }
        public int NewUsersThisMonth { get; set; }
    }
}