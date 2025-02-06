using DataAccessLayer.Respository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Respository
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly DbContext.ApplicationDbContext _context;

        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext.ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAllQueryable(string? includeProperties = null, bool tracked = false)
        {

            IQueryable<T> query = _dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            query = IncludeNavigationProperties(query);
            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, bool tracked = false)
        {

            IQueryable<T> query = _dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            query= IncludeNavigationProperties(query);
            return await query.ToListAsync();

        }

        public IQueryable<T> FindAllQueryable(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
        {

            IQueryable<T> query = _dbSet;

            if(!tracked)
                query = query.AsNoTracking();

            query = query.Where(predicate);
            query = IncludeNavigationProperties(query);

            return query;
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
        {

            IQueryable<T> query = _dbSet;

            if(!tracked)
                query = query.AsNoTracking();

            query = query.Where(predicate);
            query=IncludeNavigationProperties(query);

            return await query.ToListAsync();

        }

        public  async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
        {

            IQueryable<T> query = _dbSet;

            if(!tracked)
                query = query.AsNoTracking();

            query=IncludeNavigationProperties(query, includeProperties);
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
          
        }

        public void Remove(T entity)
        {
             _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
      
        private IQueryable<T>IncludeNavigationProperties(IQueryable<T> query,string?includeProperties=null)
        {

            if(!string.IsNullOrWhiteSpace(includeProperties))
            {
                var properites = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var property in properites)
                {
                    query = query.Include(property);
                }
            }

            return query;
        }



        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

       


    }










}
