namespace JourneyMate.Infrastracture.Data.Configurations;

public class APIEventItemConfiguration : IEntityTypeConfiguration<APIEventItem>
{
    public void Configure(EntityTypeBuilder<APIEventItem> builder)
    {

        builder.ToTable("APIEventItems");
        builder.HasIndex(e => new { e.Title, e.Date, e.Location })
               .IsUnique();
    }
}
