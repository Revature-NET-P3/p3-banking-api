﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Banking.API.Models;
using Banking.API.Repositories.Interfaces;

namespace Banking.Tests.DataObjects
{
    class AccountRepoTest : IAccountRepo
    {

        public List<Account> _accounts;
        public List<Transaction> _transactions;

        //create a mock data for test purposes.
        public AccountRepoTest(bool doFillData = true)
        {
            if (doFillData)
            {
                _accounts = new List<Account>()
                {
                    new Account() { Id = 1, UserId = 10, AccountTypeId = 3, Balance = 200, CreateDate = DateTime.Now },
                    new Account() { Id = 2, UserId = 20, AccountTypeId = 1, Balance = 300, CreateDate = DateTime.Today},
                    new Account() { Id = 3, UserId = 30, AccountTypeId = 2, Balance = 500, CreateDate = DateTime.Today },
                    new Account() { Id = 4, UserId = 30, AccountTypeId = 4, Balance = 600, CreateDate = DateTime.Now },
                    new Account() { Id = 5, UserId = 30, AccountTypeId = 1, Balance = 0, CreateDate = DateTime.Now, IsClosed = false },
                    new Account() { Id = 6, UserId = 30, AccountTypeId = 1, Balance = 0, CreateDate = DateTime.Now, IsClosed = true }

                };

                _transactions = new List<Transaction>()
                {
                    new Transaction() { Id = 1, AccountId =2, Ammount = 300, AssociatedAccountId=-1, TimeStamp=new DateTime(2000,2,14), TransactionTypeId=1},
                    new Transaction() { Id = 2, AccountId =1, Ammount = 200, AssociatedAccountId=-1, TimeStamp=new DateTime(2001,3,2), TransactionTypeId=1},
                    new Transaction() { Id = 3, AccountId =3, Ammount = 200, AssociatedAccountId=-1, TimeStamp=new DateTime(1990,10,26), TransactionTypeId=1},
                    new Transaction() { Id = 3, AccountId =3, Ammount = 100, AssociatedAccountId=-1, TimeStamp=new DateTime(1990,11,15), TransactionTypeId=2},
                    new Transaction() { Id = 4, AccountId =3, Ammount = 200, AssociatedAccountId=-1, TimeStamp=new DateTime(1990,12,20), TransactionTypeId=2},
                    new Transaction() { Id = 5, AccountId =4, Ammount = 600, AssociatedAccountId=-1, TimeStamp=new DateTime(2002,5,5), TransactionTypeId=1},
                };
            }
        }


        // used for testing Close Account method.
        public async Task<bool> CloseAccount(int Id)
        {
            var accToClose = _accounts.FirstOrDefault(e => e.Id == Id);
            await Task.Delay(10);
            if (accToClose != null)
            {
                accToClose.IsClosed = true;
                return true;
            }
            return false;
        }

        // used for testing Deposit method.
        public async Task<bool> Deposit(int Id, decimal amount)
        {
            var depositAccount = _accounts.FirstOrDefault(e => e.Id == Id);
            await Task.Delay(10);
            if (depositAccount.IsClosed)
            {
                return false;
            }
            else if (depositAccount != null)
            {
                depositAccount.Balance += amount;
                return true;
            }
            return false;
        }

        // used for testing Account Details method.
        public async Task<Account> GetAccountDetailsByAccountID(int Id)
        {
            var accDetails = _accounts.Where(e => e.Id == Id).SingleOrDefault();
            await Task.Delay(10);
            return accDetails;
        }

        // used for testing Get All Account method.
        public async Task<IEnumerable<Account>> GetAllAccountsByUserId(int UserId)
        {
            var account = _accounts.Where(e => e.UserId == UserId).ToList();
            await Task.Delay(10);
            return account;
        }

        // used for testing Get All Account by User ID and Account Type method.
        public async Task<IEnumerable<Account>> GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId)
        {
            var accByType = _accounts.Where(e => e.UserId == UserId && e.AccountTypeId == AccountTypeId).ToList();
            await Task.Delay(10);
            return accByType;
        }


        // used for testing Get All Transactions by Account ID method.
        public async Task<IEnumerable<Transaction>> GetTransactionDetailsByAccountID(int Id)
        {
            var transactionDetails = _transactions.Where(e => e.AccountId == Id).ToList();
            await Task.Delay(10);
            return transactionDetails;
        }

        // used for testing Open Account method.
        public async Task<Account> OpenAccount(Account account)
        {
            var accounts = _accounts.FirstOrDefault(e => e.Id == account.Id);
            await Task.Delay(10);
            if (accounts == null)
            {
                account.Id = _accounts.Max(a => a.Id) + 1;
                _accounts.Add(account);
                return account;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Overdraft(int Id, decimal amount)
        {
            _transactions.Add(
            new Transaction()
            {
                Id = _transactions.Max(tr => tr.Id) + 1,
                Ammount = amount,
                AccountId = Id,
                TimeStamp = DateTime.Now,
            });
            await Task.Delay(10);

            return true;
        }

        // used for testing Pay Loan method.
        public async Task<bool> PayLoan(int Id, decimal amount)
        {
            var loanAccount = _accounts.FirstOrDefault(e => e.Id == Id);
            await Task.Delay(10);
            if (loanAccount.IsClosed)
            {
                return false;
            }
            if (loanAccount != null)
            {
                loanAccount.Balance = loanAccount.Balance <= amount ? 0 : loanAccount.Balance - amount;
                return true;
            }
            return false;
        }

        public async Task<bool> SaveChanges()
        {
            await Task.Delay(10);
            return true;
        }


        // used for testing Transfer method.
        public async Task<bool> TransferBetweenAccounts(int Id, decimal fromAmount, int toAccId, decimal toAmount)
        {
            var transferAccount = _accounts.FirstOrDefault(e => e.Id == Id);
            var accountTo = _accounts.FirstOrDefault(m => m.Id == toAccId);
            await Task.Delay(10);

            //check that account is open
            if (transferAccount.IsClosed || accountTo.IsClosed)
            {
                return false;
            }

            if (transferAccount != null && accountTo != null)
            {
                if (transferAccount.AccountTypeId == 2)
                {
                    transferAccount.Balance -= fromAmount;
                }
                else
                {
                    transferAccount.Balance = transferAccount.Balance <= fromAmount ? 0 : transferAccount.Balance - fromAmount;
                }
                accountTo.Balance += toAmount;
                return true;
            }
            return false;
        }


        // used for testing Withdraw method.
        public async Task<bool> Withdraw(int Id, decimal amount)
        {
            var withdrawAccount = _accounts.FirstOrDefault(e => e.Id == Id);
            await Task.Delay(10);

            if (withdrawAccount.IsClosed)
            {
                return false;
            }

            if (withdrawAccount != null)
            {
                if (withdrawAccount.AccountTypeId == 2)
                {
                    withdrawAccount.Balance -= amount;
                }
                else
                {
                    withdrawAccount.Balance = withdrawAccount.Balance <= amount ? 0 : withdrawAccount.Balance - amount;
                }
                return true;
            }
            return false;
        }
    }
}