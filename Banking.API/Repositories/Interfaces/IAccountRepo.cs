using System.Collections.Generic;
using System.Threading.Tasks;

using Banking.API.Models;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        Task<Account> OpenAccount(Account account);
        Task<bool> Deposit(int Id, decimal amount);
        Task<bool> Withdraw(int Id, decimal amount);
        Task<bool> TransferBetweenAccounts(int Id, decimal fromAmount, int toAccId, decimal toAmount);
        Task<bool> PayLoan(int Id, decimal amount);
        Task<bool> CloseAccount(int Id);
        Task<IEnumerable<Account>> GetAllAccountsByUserId(int UserId);
        Task<IEnumerable<Account>> GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId);
        Task<Account> GetAccountDetailsByAccountID(int Id);
        Task<IEnumerable<Transaction>> GetTransactionDetailsByAccountID(int Id);
        Task<bool> SaveChanges();
        Task<bool> Overdraft(int Id, decimal amount);
    }
}
