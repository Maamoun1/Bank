namespace DataAccessLayer.Respository.IRepository
{
    public interface IReadOnlyRepostitory<T> where T:class
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
