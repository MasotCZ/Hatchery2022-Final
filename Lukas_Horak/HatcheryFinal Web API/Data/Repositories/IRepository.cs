namespace HatcheryFinal_Web_API.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity);
        public void Remove(TEntity entity);

        public Task<int> SaveChangesAsync();
    }
}
