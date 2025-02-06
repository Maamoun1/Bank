using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class ReadOnlyService<T> : IReadOnlyService<T> where T : class
    {

        private readonly IReadOnlyRepostitory<T> _readOnlyRepostitory;

        public ReadOnlyService(IReadOnlyRepostitory<T> readOnlyRepostitory)
        {
            _readOnlyRepostitory = readOnlyRepostitory;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
           return _readOnlyRepostitory.GetAllAsync();
        }
    }
}
