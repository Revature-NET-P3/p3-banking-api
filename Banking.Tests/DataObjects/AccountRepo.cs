using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Tests.DataObjects
{
    class AccountRepo : IAccountRepo
    {
        List<Account> Accounts;

        public AccountRepo(bool doFillData = true)
        {

        }

        public Account CloseAccount(int Id)
        {
            throw new NotImplementedException();
        }

        public Account Deposit(int Id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountDetailsByAccountID(int Id)
        {
            throw new NotImplementedException();
        }

        public Account GetAllAccountsByUserId(int UserId)
        {
            throw new NotImplementedException();
        }

        public Account GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId)
        {
            throw new NotImplementedException();
        }

        public Account GetTransactionDetailsByAccountID(int Id)
        {
            throw new NotImplementedException();
        }

        public Account OpenAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account PayLoan(int Id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Account TransferBetweenAccounts(int Id, decimal amount, int toAccId)
        {
            throw new NotImplementedException();
        }

        public Account Withdraw(int Id, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
