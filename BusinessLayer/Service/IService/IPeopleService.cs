using BusinessLayer.DTOs.People;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public interface IPersonService : IGenericeService<TbPerson>
    {
        Task Update(int personID, UpdatePersonDto dto);
        bool IsPersonExist(int PersonID);

        Task AddPerson(CreatePersonDto dto);


    }
}
