using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Repositories
{
    public class RateCopanyRepository:GenericRepository<UserRating> , IRateCopanyRepository
    {

        public RateCopanyRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<UserRating?> FindAsync(Expression<Func<UserRating, bool>> predicate)
        {
            return await _dbContext.Set<UserRating>().FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<UserRating>> WhereAsync(Expression<Func<UserRating, bool>> predicate)
        {
            return await _dbContext.Set<UserRating>().Where(predicate).ToListAsync();
        }
    }
}
