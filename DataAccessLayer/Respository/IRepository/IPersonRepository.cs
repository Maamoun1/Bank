

namespace DataAccessLayer.Respository.IRepository
{
    public interface IPersonRepostitory : IGenericRepository<TbPerson>
    {
        void Update(TbPerson entity);
        bool IsPersonExist(int PersonID);
        Task SaveChanges();

    }



}
