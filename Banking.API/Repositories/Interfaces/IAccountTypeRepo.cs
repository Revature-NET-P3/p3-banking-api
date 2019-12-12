using Banking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountTypeRepo
    {
<<<<<<< HEAD
        Task<IEnumerable<AccountType>> GetAccountTypes();
        Task<AccountType> GetAccountTypeById(int id);
        Task<AccountType> GetAccountTypeByName(string name);
        Task<bool> AddAccountType(AccountType accType);
=======
        public Task<List<AccountType>> GetAccountTypes();
        public Task<AccountType> GetAccountTypeById(int id);
        public Task<AccountType> GetAccountTypeByName(string name);
        public Task<bool> AddAccountType(AccountType accType);
>>>>>>> d20dbd8a002f5ec154b83448e275fcc492db82dc
    }
}