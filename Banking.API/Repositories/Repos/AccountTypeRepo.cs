using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

using Banking.API.Models;
using Banking.API.Repositories.Interfaces;

namespace Banking.API.Repositories
{
    public class AccountTypeRepo : IAccountTypeRepo
    {
        private AppDbContext _context;

        public AccountTypeRepo(AppDbContext ctx)
        {
            _context = ctx;
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