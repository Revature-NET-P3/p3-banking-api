using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.async Tasks;
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

        public async Task<bool> AddAccountType(AccountType accType)
        {
            bool result = false;

            var query = AccountTypes.Where(at => at.Name.ToUpper().CompareTo(accType.Name.ToUpper()) != 0);
            if (query.Count() < 1)
            {
                AccountTypes.Add(accType);
                await Task.Delay(10);
                result = true;
            }

            return result;
        }

        public async Task<AccountType> GetAccountTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountType> GetAccountTypeByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AccountType>> GetAccountTypes()
        {
            throw new NotImplementedException();
        }
    }
}