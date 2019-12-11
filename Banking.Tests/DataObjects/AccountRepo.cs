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
