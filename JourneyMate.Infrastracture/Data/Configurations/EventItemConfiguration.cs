namespace JourneyMate.Infrastracture.Data.Configurations;

public class EventItemConfiguration : IEntityTypeConfiguration<EventItem>
{
    public void Configure(EntityTypeBuilder<EventItem> builder)
    {

        builder.ToTable("EventItems");
        builder.HasIndex(e => new { e.Title, e.Date, e.Location })
         .IsUnique();
    }
}
