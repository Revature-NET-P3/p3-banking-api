﻿using Banking.API.Models;
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
        public Task<bool> TransferBetweenAccounts(int Id, decimal amount, int toAccId);
        public Task<bool> PayLoan(int Id, decimal amount);
        public Task<bool> CloseAccount(int Id);
        public Task<bool> GetAllAccountsByUserId(int UserId);
        public Task<bool> GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId);
        public Task<bool> GetAccountDetailsByAccountID(int Id);
        public Task<bool> GetTransactionDetailsByAccountID(int Id);
    }
}
