using System.Linq.Expressions;

namespace JourneyMate.Core.Interfaces;

public interface IGenericRepository<T> where T : ModelBase
{
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>?> GetAllAsync();
    T? GetById(int id);
    Task<T?> GetByIdAsync(int id);
    // return int >> return number of row effected
    void Add(T entity);
    Task AddAsync(T entity);
    Task AddRangeAsync(List<T> entity);
    void Update(T entity);
    void Delete(T entity);

    Task<IReadOnlyCollection<T>?> GetAllWithSpecificaiton(ISpec<T> specefication);

    Task<T?> GetByIdWithSpecifications(ISpec<T> specification);

    Task<int> GetCountSpecAsync(ISpec<T> specefication);
    Task<int> CountSpecAsync(ISpec<T> specefication);
    Task<IReadOnlyList<T>> GetByIdsTravelAsync(IEnumerable<int> ids);
    Task<IReadOnlyList<T>> GetByIdsEventAsync(IEnumerable<int> ids);

}
