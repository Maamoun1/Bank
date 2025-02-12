using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository.IRepository
{
    public interface IAccountRepository : IGenericRepository<TbAccount>
    {
        Task DecativeAccount(int  accountId);

        bool IsAccountExist(string pinCode);
        void Update(TbAccount account);

        void Deposite(string pinCode, double balance);
        void Withdraw(string pinCode, double balance);

        double GetBalance(string pinCode);

        Task<bool> TransferAmountAsync(string senderId, string receviedId, double amount);

    }
}
