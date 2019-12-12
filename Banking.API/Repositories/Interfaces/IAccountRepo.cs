using Banking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        public Task<Account> OpenAccount(Account account);
        public Task<bool> Deposit(int Id, decimal amount);
        public Task<bool> Withdraw(int Id, decimal amount);
        public Task<bool> TransferBetweenAccounts(int Id, decimal fromAmount, int toAccId, decimal toAmount);
        public Task<bool> PayLoan(int Id, decimal amount);
        public Task<bool> CloseAccount(int Id);
        public Task<IEnumerable<Account>> GetAllAccountsByUserId(int UserId);
        public Task<IEnumerable<Account>> GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId);
        public Task<bool> GetAccountDetailsByAccountID(int Id);
        public Task<bool> GetTransactionDetailsByAccountID(int Id);
    }
}
