using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JourneyMate.Infrastracture.Specefications;
using JourneyMate.Core.Models.Company_Aggregation;
namespace JourneyMate.Infrastracture.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _dbContext;
        public GenericRepository(AppDbContext dbContext) => _dbContext = dbContext;
        public void Add(T entity) => _dbContext.Set<T>().Add(entity);
        public async Task AddAsync(T entity) => await _dbContext.Set<T>().AddAsync(entity);
public async Task AddRangeAsync(List<T> entity) => await _dbContext.Set<T>().AddRangeAsync(entity);
public void Update(T entity) => _dbContext.Set<T>().Attach(entity);
        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
        public T? GetById(int id) => _dbContext.Set<T>().Find(id);
        public async Task<T?> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

        public IEnumerable<T> GetAll() => _dbContext.Set<T>().AsNoTracking().ToList();

        //edit by Abdallah
        public async Task<IEnumerable<T>?> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyCollection<T>?> GetAllWithSpecificaiton(ISpec<T> specefication)
        {
            if (specefication is not null)
            {
                return await SpeceficationQueryEvaluator<T>.GetQuery(_dbContext.Set<T>(), specefication).ToListAsync();
            }
            return null;
        }
        public async Task<T?> GetByIdWithSpecifications(ISpec<T> specification)
        {
            if (specification is not null)
            {
                return await SpeceficationQueryEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification).FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<int> GetCountSpecAsync(ISpec<T> specefication)
        {

            return await SpeceficationQueryEvaluator<T>.GetQuery(_dbContext.Set<T>(), specefication).CountAsync();
        }

        public async Task<int> CountSpecAsync(ISpec<T> specefication)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (specefication.Criteria != null)
            {
                query = query.Where(specefication.Criteria);
            }
            return await query.CountAsync();
        }
        public async Task<IReadOnlyList<T>> GetByIdsTravelAsync(IEnumerable<int> ids)
        {
            return  await _dbContext.Set<T>().Include("Company").Include("itineraries")
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetByIdsEventAsync(IEnumerable<int> ids)
        {
            return await _dbContext.Set<T>()
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();
        }

    }
}
