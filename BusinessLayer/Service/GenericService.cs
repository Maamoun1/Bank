using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Service.IService;
using DataAccessLayer.Respository;
using DataAccessLayer.Respository.IRepository;
using System.Linq.Expressions;

namespace BusinessLayer.Service
{
    public class GenericService<T> : IGenericeService<T> where T : class
    {

        private readonly IGenericRepository<T> _Repository;
        
        public GenericService(IGenericRepository<T> repository)
        {
            _Repository = repository;
        }
       
        public virtual async Task AddAsync(T Entity)
        {
            await _Repository.AddAsync(Entity);
            
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
        {
            return await _Repository.FindAllAsync(predicate, includeProperties, tracked);
        }

        public IQueryable<T> FindAllQueryable(System.Linq.Expressions.Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
        {
           return _Repository.FindAllQueryable(predicate, includeProperties, tracked);
        }

        public async Task<IEnumerable<T>> GetAllAsyncc(string? includeProperties = null, bool tracked = false)
        {
            
            return await _Repository.GetAllAsync(includeProperties, tracked);

        }

        public IQueryable<T> GetAllQueryable(string? includeProperties = null, bool tracked = false)
        {
           return _Repository.GetAllQueryable(includeProperties, tracked);
        }

        public async Task<T?> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, string? includeProperties = null, bool tracked = false)
        {
           
            return await (_Repository.GetAsync(predicate,includeProperties,tracked));

        }
 
        public void RemoveRange(IEnumerable<T> entities)
        {
            _Repository.RemoveRange(entities);

        }

        public  void Remove(T Entity)
        {
            _Repository.Remove(Entity);
            
        }

        public async Task SaveChanges()
        {
           await _Repository.SaveChanges();
        }
    }
}
