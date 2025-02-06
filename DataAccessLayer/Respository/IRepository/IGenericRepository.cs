

namespace DataAccessLayer.Respository.IRepository
{

    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllQueryable(string? includeProperties = null, bool tracked = false); //IQueryable<T>

        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, bool tracked = false); //Task<IEnumerable<T>>

        IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false);

        Task AddAsync(T Entity);
        void Remove(T Entity);

        void RemoveRange(IEnumerable<T> entities);

        Task SaveChanges();

    }







}
