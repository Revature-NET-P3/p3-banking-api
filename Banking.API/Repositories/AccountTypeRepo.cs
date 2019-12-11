using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories
{
    public class AccountTypeRepo : IAccountTypeRepo
    {
        private AppDbContext _context;

        public AccountTypeRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<IEnumerable<AccountType>> GetAccountTypes()
        {
            var AccountTypes = await _context.AccountType.ToListAsync();
            return AccountTypes;
        }
        public async Task<AccountType> GetAccountTypeById(int id)
        {
            AccountType accType = await _context.AccountType.FirstOrDefaultAsync(o => o.Id == id);
            return accType;
        }
        public async Task<AccountType> GetAccountTypeByName(string name)
        {
            AccountType accType = await _context.AccountType.FirstOrDefaultAsync(x => x.Name == name);
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