using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JourneyMate.Infrastracture.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
       

        builder.HasIndex(e => e.UserId)
               .IsUnique();

        builder.HasMany(c => c.Ratings)
                .WithOne(ur => ur.Company)
                .HasForeignKey(ur => ur.CompanyId)
                .OnDelete(DeleteBehavior.Cascade); // Ratings will be deleted if the company is deleted

        //// Configure relationship with SocialMediaLinks
        builder.HasMany(c => c.SocialMediaLinks)
            .WithOne(sml => sml.Company)
            .HasForeignKey(sml => sml.CompanyId)
            .OnDelete(DeleteBehavior.Cascade); // SocialMediaLinks will be deleted if the company is deleted

        //// Configure relationship with PaymentMethods
        builder.HasMany(c => c.PaymentMethods)
            .WithOne(pm => pm.Company)
            .HasForeignKey(pm => pm.CompanyId)
            .OnDelete(DeleteBehavior.Cascade); // PaymentMethods will be deleted if the company is deleted

        builder.HasMany(c => c.Travels)
                .WithOne(t => t.Company)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Cascade); // Travels will be deleted if the company is deleted

        builder.OwnsMany(c => c.WorkingHours, wh =>
        {
            wh.WithOwner().HasForeignKey("CompanyId");
            wh.Property(w => w.DayOfWeek).IsRequired();
            wh.Property(w => w.OpenTime);
            wh.Property(w => w.CloseTime);
        });
    }
}
