namespace HatcheryFinal_Web_API.Data.Repositories
{
    /// <summary>
    /// Base repository with basic Dbcontext methods.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds incoming entity to the DbContext.
        /// </summary>
        /// <param name="entity">entity to add</param>
        public void Add(TEntity entity);

        /// <summary>
        /// Removes incoming entity  from DbContext.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity);

        /// <summary>
        /// Saves changes to the DbContext.
        /// All changes before hand are local. 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync();
    }
}
