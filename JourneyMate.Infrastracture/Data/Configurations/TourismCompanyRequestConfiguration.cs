namespace JourneyMate.Infrastracture.Data.Configurations;

public class TourismCompanyRequestConfiguration : IEntityTypeConfiguration<TourismCompanyRequest>
{
    public void Configure(EntityTypeBuilder<TourismCompanyRequest> builder)
    {

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasAnnotation("RegularExpression", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        builder.Property(x => x.PhoneNumber).IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.ContactPersonName).IsRequired();
    }
}
