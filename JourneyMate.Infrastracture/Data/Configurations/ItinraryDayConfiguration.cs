using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Data.Configurations
{
    public class ItinraryDayConfiguration : IEntityTypeConfiguration<ItinraryDay>
    {
        public void Configure(EntityTypeBuilder<ItinraryDay> builder)
        {
            builder.Property(t => t.Activities)
            .HasColumnType("NVARCHAR(MAX)")
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
            );
        }
    }
}
