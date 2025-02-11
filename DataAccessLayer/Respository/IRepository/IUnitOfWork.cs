
namespace DataAccessLayer.Respository.IRepository
{
    public interface IUnitOfWork 
    {
        IPersonRepostitory Person {  get;}
        IUserRepository User { get;}
        IAccountsTypesRepository AccountType { get; }
        IApplicationsRepository Applications { get; }
        IAccountRepository Account { get; }

        Task SaveAsync();
    }
}
