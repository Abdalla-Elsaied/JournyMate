using JourneyMate.Core.Models.Company_Aggregation.Travel_Aggregation;

namespace JourneyMate.Infrastracture.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Travel> Travel { get; set; }
    public DbSet<TourismCompanyRequest>  tourismCompanyRequests { get; set; }
    public DbSet<APIEventItem> APIEventItems { get; set; }
    public DbSet<EventItem> EventItems { get; set; }
    public DbSet<FavoriteTravel> FavoriteTravel { get; set; }  

    public DbSet<Booking> bookings { get; set; }

    public DbSet<Image> Images { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
