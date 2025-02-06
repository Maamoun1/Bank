using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public interface IAccountTypeService
    {
        Task<IEnumerable> GetAccountTypeAsync();

    }
}
