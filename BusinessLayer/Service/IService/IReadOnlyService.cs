using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public interface IReadOnlyService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

    }
}
