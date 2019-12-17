using System.Collections.Generic;
using System.Threading.Tasks;

using Banking.API.Models;

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