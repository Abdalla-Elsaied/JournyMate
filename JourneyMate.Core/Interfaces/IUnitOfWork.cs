using JourneyMate.Core.Models.Company_Aggregation;

namespace JourneyMate.Core.Interfaces;

public interface IUnitOfWork:IAsyncDisposable
{
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : ModelBase;
    public IRateCopanyRepository  RateRepo { get; set; }
    public IFavTravelRepository  FavTravelRepo { get; set; }
    public IItinraryDayRepo itinraryDayRepo { get; set; }
    Task<int> CompleteAsync();
    int Complete();
}