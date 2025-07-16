using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Core.Interfaces
{
    public interface IRateCopanyRepository:IGenericRepository<UserRating>
    {
        Task<UserRating?> FindAsync(Expression<Func<UserRating, bool>> predicate);
        Task<IEnumerable<UserRating>> WhereAsync(Expression<Func<UserRating, bool>> predicate);
    }
}
