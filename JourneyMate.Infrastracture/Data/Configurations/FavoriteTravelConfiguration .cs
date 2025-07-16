using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Data.Configurations
{
    public class FavoriteTravelConfiguration : IEntityTypeConfiguration<FavoriteTravel>
    {
        public void Configure(EntityTypeBuilder<FavoriteTravel> builder)
        {
        
            builder.HasKey(ft => new { ft.UserId, ft.TravelId });

           
            builder.HasOne(ft => ft.User)
                .WithMany(u => u.FavoriteTravels)
                .HasForeignKey(ft => ft.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

           
            builder.HasOne(ft => ft.Travel)
                .WithMany(t => t.FavoritedByUsers)
                .HasForeignKey(ft => ft.TravelId)
                .OnDelete(DeleteBehavior.Cascade); 

           
            builder.Property(ft => ft.FavoritedOn)
                .HasDefaultValueSql("GETDATE()"); 
        }
    }
}
