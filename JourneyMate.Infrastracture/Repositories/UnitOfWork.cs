using System.Collections;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private Hashtable _repository;
        public IRateCopanyRepository RateRepo { get; set; }
        public IFavTravelRepository FavTravelRepo { get; set; }
        public IItinraryDayRepo itinraryDayRepo { get; set ; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = new Hashtable();
            RateRepo = new RateCopanyRepository(_dbContext);
            FavTravelRepo = new FavTravelRepository(_dbContext);
            itinraryDayRepo = new ItinraryDayRepo(_dbContext);
        }
        public IGenericRepository<Tentity> Repository<Tentity>() where Tentity : ModelBase
        {
            var Key = typeof(Tentity).Name;
            if (!_repository.ContainsKey(Key))
            {
                var repository = new GenericRepository<Tentity>(_dbContext);
                _repository.Add(Key, repository);
            }
            return _repository[Key] as IGenericRepository<Tentity>;
        }
        public int Complete()=>  _dbContext.SaveChanges();
        public void Dispose() => _dbContext.Dispose();
        public async Task<int> CompleteAsync()=> await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();
    }
}
