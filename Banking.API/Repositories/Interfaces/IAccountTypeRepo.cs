using Banking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Interfaces
{
    interface IAccountTypeRepo
    {
        public Task<IEnumerable<AccountType>> GetAccountTypes();
        public Task<AccountType> GetAccountTypeById();
        public Task<AccountType> GetAccountTypeByName();
        public Task<bool> AddAccountType();
    }
}