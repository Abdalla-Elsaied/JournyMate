namespace JourneyMate.Infrastracture.Data.Configurations;

public class SocialMediaConfiguration : IEntityTypeConfiguration<SocialMediaLink>
{
    public void Configure(EntityTypeBuilder<SocialMediaLink> builder)
    {

        builder.HasOne(t => t.Company)
               .WithMany(c => c.SocialMediaLinks)
               .HasForeignKey(t => t.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
