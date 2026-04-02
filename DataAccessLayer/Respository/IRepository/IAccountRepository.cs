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

       Task <bool> IsAccountExist(string pinCode);
        void Update(TbAccount account);

        Task DepositeAsync(string accountNumber, decimal balance);
        Task WithdrawAsync(string accountNumber, decimal balance);

        Task<decimal?> GetBalanceAsync(string accountNumber);

        Task<bool> TransferAmountAsync(string senderId, string receviedId, decimal amount);

    }
}
