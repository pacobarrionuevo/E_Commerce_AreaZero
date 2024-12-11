namespace E_Commerce_VS.Models.Database.Repositories
{
    public interface IRepositorio<TEntity> where TEntity : class
    {
        //Calcadisimo de los ejemplos de Jose
        Task<ICollection<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetQueryable(bool asNoTracking = true);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> ExistsAsync(object id);
    }
}
