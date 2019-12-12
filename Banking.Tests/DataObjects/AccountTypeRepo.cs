using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Tests.DataObjects
{
    public class AccountTypeRepo : IAccountTypeRepo
    {
        List<AccountType> accountTypes;

        public AccountTypeRepo()
        {
            List<AccountType> accountTypes = new List<AccountType>()
            {
                new AccountType {}
            };
        }

        public async Task<List<AccountType>> GetAccountTypes()
        {
            return await _context.AccountTypes.ToListAsync();
        }
        public async Task<AccountType> GetAccountTypeById(int id)
        {
            AccountType accType = await _context.AccountTypes.FirstOrDefaultAsync(o => o.Id == id);
            return accType;
        }
        public async Task<AccountType> GetAccountTypeByName(string name)
        {
            AccountType accType = await _context.AccountTypes.FirstOrDefaultAsync(x => x.Name == name);
            return accType;
        }
        public async Task<bool> AddAccountType(AccountType accType)
        {
            _context.Add(accType);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
