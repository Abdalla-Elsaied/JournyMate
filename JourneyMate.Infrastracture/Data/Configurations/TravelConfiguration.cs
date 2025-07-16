using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JourneyMate.Infrastracture.Data.Configurations;

public class TravelConfiguration : IEntityTypeConfiguration<Travel>
{
    public void Configure(EntityTypeBuilder<Travel> builder)
    {
        builder
          .HasOne(t => t.category)
          .WithMany(C=>C.Travels)
          .HasForeignKey(t => t.CategoryId)
          .IsRequired();

        builder.Property(t => t.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Description)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(t => t.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.HasOne(t => t.Company)
               .WithMany(c => c.Travels)
               .HasForeignKey(t => t.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.DeparturePointLat)
            .HasColumnType("float");

        builder.Property(t => t.DeparturePointLng)
            .HasColumnType("float");

        builder.Property(t => t.Amenities)
       .HasColumnType("NVARCHAR(MAX)")
       .HasConversion(
           v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
           v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
       );
        builder.HasMany(x=>x.itineraries).WithOne(x=>x.Travel).HasForeignKey(x=>x.TravelId).OnDelete(DeleteBehavior.Cascade);

      
    }
}
