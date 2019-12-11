using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banking.Tests.DataObjects
{
    class AccountRepo : IAccountRepo
    {
        List<Account> Accounts;
        List<Transaction> Transactions;

        public AccountRepo(bool doFillData = true)
        {
            if (doFillData)
            {
                Accounts = new List<Account>()
                {
                    new Account()
                    {
                        Id = 1,
                         UserId = 1,
                          AccountTypeId = 1,
                           Balance = 100.0M,
                            CreateDate = DateTime.Now
                    },
                    new Account()
                    {
                        Id = 2,
                         UserId = 1,
                          AccountTypeId = 2,
                           Balance = 200.0M,
                            CreateDate = DateTime.Now
                    },
                    new Account()
                    {
                        Id = 3,
                         UserId = 3,
                          AccountTypeId = 1,
                           Balance = 1000.0M,
                            CreateDate = DateTime.Now
                    },
                    new Account()
                    {
                        Id = 4,
                         UserId = 3,
                          AccountTypeId = 3,
                           Balance = 2000.0M,
                            CreateDate = DateTime.Now
                    },
                    new Account()
                    {
                        Id = 5,
                         UserId = 3,
                          AccountTypeId = 4,
                           Balance = 2000.0M,
                            CreateDate = DateTime.Now
                    },
                };

                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                         Id = 1,
                          AccountId=1,
                           TransactionTypeId=1,
                            Ammount=100,
                    }
                };
            }
        }

        public Account CloseAccount(int Id)
        {
            Account result = null;

            var query = Accounts.Where(a => a.Id == Id);
            if (query.Count() > 0)
            {
                result = query.First();
                // Task.Delay(10);
                Accounts.Remove(result);
            }

            return result;
        }

        public Account Deposit(int Id, decimal amount)
        {
            Account result = null;

            if (amount > 0)
            {
                var query = Accounts.Where(a => a.Id == Id && (a.AccountTypeId == 1 || a.AccountTypeId == 2));
                if (query.Count() > 0)
                {
                    result = query.First();
                    result.Balance += amount;
                }
            }

            return result;
        }

        public Account GetAccountDetailsByAccountID(int Id)
        {
            Account result = null;

            var query = Accounts.Where(a => a.Id == Id);
            if (query.Count() > 0)
            {
                result = query.First();
            }

            return result;
        }

        public Account GetAllAccountsByUserId(int UserId)
        {
            //List<Account> result = null;

            //var query = Accounts.Where(a => a.UserId == UserId);
            //if (query.Count() > 0)
            //{
            //    result = query.ToList();
            //}

            //return result;
            return null;
        }

        public Account GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId)
        {
            //List<Account> result = null;

            //var query = Accounts.Where(a => a.UserId == UserId && a.AccountTypeId == AccountTypeId);
            //if (query.Count() > 0)
            //{
            //    result = query.ToList();
            //}

            //return result;
            return null;
        }

        public Account GetTransactionDetailsByAccountID(int Id)
        {
            //List<Transaction> result = null;

            //var query = Transactions.Where(t => t.AccountId == Id);
            //if (query.Count() > 0)
            //{
            //    result = query.ToList();
            //}

            //return result;
            return null;
        }

        public Account OpenAccount(Account account)
        {
            Account result = null;

            var query = Accounts.Where(a => a.Id == account.Id);
            if (query.Count() < 1)
            {
                int max = Accounts.Max(a => a.Id);
                account.Id = max;
                Accounts.Add(account);
                result = account;
            }

            return result;
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
