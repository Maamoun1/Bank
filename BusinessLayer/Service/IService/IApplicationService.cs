using BusinessLayer.DTOs.Applications;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public interface IApplicationService : IGenericeService<TbApplication>,IReadOnlyService<TbApplication>
    {
        public void Cancel(TbApplication application);
        public void SetComplete(TbApplication application);

       Task Add(CreateApplicationDto applicationDto);

    }
}
