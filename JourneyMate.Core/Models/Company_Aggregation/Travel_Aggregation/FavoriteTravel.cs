using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Models.Company_Aggregation
{
    public class FavoriteTravel:ModelBase
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int TravelId { get; set; }
        public Travel Travel { get; set; }

        public DateTime FavoritedOn { get; set; } = DateTime.UtcNow;
    }
}
