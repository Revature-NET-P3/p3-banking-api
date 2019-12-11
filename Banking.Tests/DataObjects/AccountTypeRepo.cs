using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests.DataObjects
{
    class AccountTypeRepo : IAccountTypeRepo
    {
        List<AccountType> AccountTypes = null;

        public AccountTypeRepo(bool doFill = true)
        {
            if (doFill)
            {
                AccountTypes = new List<AccountType>()
                {
                    new AccountType()
                    {
                         Id = 1,
                         InterestRate = 1.0M,
                         Name = "Checking",
                    },
                    new AccountType()
                    {
                         Id = 2,
                         InterestRate = 2.0M,
                         Name = "Business",
                    },
                    new AccountType()
                    {
                         Id = 3,
                         InterestRate = 3.0M,
                         Name = "Loan",
                    },
                    new AccountType()
                    {
                         Id = 4,
                         InterestRate = 4.0M,
                         Name = "Term CD",
                    },
                };
            }
        }

        public Task<bool> AddAccountType(AccountType accType)
        {
            throw new NotImplementedException();
        }

        public Task<AccountType> GetAccountTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountType> GetAccountTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountType>> GetAccountTypes()
        {
            throw new NotImplementedException();
        }
    }
}