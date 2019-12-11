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