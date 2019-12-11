using Banking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        Account OpenAccount(Account account);
        Account Deposit(int Id, decimal amount);
        Account Withdraw(int Id, decimal amount);
        Account TransferBetweenAccounts(int Id, decimal amount, int toAccId);
        Account PayLoan(int Id, decimal amount);
        Account CloseAccount(int Id);
        Account GetAllAccountsByUserId(int UserId);
        Account GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId);
        Account GetAccountDetailsByAccountID(int Id);
        Account GetTransactionDetailsByAccountID(int Id);
        //IEnumerable<Account> GetAllAccount();
        //Account UpdateAccount(Account accountChanges);
    }
}
