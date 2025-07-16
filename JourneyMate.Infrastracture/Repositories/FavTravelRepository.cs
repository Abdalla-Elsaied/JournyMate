using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Repositories
{
    public class FavTravelRepository:GenericRepository<FavoriteTravel> ,IFavTravelRepository
    {
        public FavTravelRepository(AppDbContext dbContext) : base(dbContext) { }
        public async Task<bool> AnyAsync(Expression<Func<FavoriteTravel, bool>> predicate) => await _dbContext.Set<FavoriteTravel>().AnyAsync(predicate);
        public async Task<List<FavoriteTravel>?> FavtrvelForUser(string usedId)
               => await _dbContext.Set<FavoriteTravel>()
                .Where(fav => fav.UserId == usedId)
                .ToListAsync();


        public async Task<FavoriteTravel?> FirstOrDefaultAsync(Expression<Func<FavoriteTravel, bool>> predicate) => await _dbContext.Set<FavoriteTravel>().FirstOrDefaultAsync(predicate);
    }
}
