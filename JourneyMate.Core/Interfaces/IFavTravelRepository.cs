using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Interfaces
{
    public interface IFavTravelRepository:IGenericRepository<FavoriteTravel>
    {
        Task<bool> AnyAsync(Expression<Func<FavoriteTravel, bool>> predicate);
        Task<FavoriteTravel?> FirstOrDefaultAsync(Expression<Func<FavoriteTravel, bool>> predicate);

        Task<List<FavoriteTravel>?> FavtrvelForUser(string usedId);

    }
}
