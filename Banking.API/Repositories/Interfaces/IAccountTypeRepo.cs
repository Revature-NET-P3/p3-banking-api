using Banking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountTypeRepo
    {
        Task<List<AccountType>> GetAccountTypes();
        Task<AccountType> GetAccountTypeById(int id);
        Task<AccountType> GetAccountTypeByName(string name);
        Task<bool> AddAccountType(AccountType accType);
    }
}