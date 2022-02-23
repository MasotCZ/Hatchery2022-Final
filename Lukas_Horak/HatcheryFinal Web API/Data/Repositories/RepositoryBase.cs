using HatcheryFinal_Web_API.Data.Repositories;

namespace HatcheryFinal_Web_API.Data
{
    abstract class RepositoryBase<_T, _L> : IRepository<_T> where _T : class
    {
        protected readonly IBankDbContext _context;
        protected readonly ILogger<_L> _logger;

        protected RepositoryBase(IBankDbContext context, ILogger<_L> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add(_T entity)
        {
            _logger.LogInformation($"Adding entity of type {entity.GetType()}");
            _context.Add(entity);
        }

        public void Remove(_T entity)
        {
            _logger.LogInformation($"Removeing entity of type {entity.GetType()}");
            _context.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            var updated = _context.SaveChangesAsync();
            _logger.LogInformation($"Saving {updated.Result} changes");
            return updated.Result;
        }
    }
}
