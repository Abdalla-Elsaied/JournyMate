using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Repositories
{
    public class ItinraryDayRepo : GenericRepository<ItinraryDay>, IItinraryDayRepo
    {
        public ItinraryDayRepo(AppDbContext dbContext) : base(dbContext) { }
        public async Task<List<ItinraryDay>> GetItinerariesByTravelIdWithInclude(int TravelId)
        {
           return await _dbContext.Set<ItinraryDay>().Where(D=>D.TravelId == TravelId).ToListAsync();
        }
    }
}
