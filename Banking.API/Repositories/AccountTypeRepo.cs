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

        public Task<IEnumerable<AccountType>> GetAccountTypes()
        {
            var AccountTypes = await _context.AccountType
        }
        public Task<AccountType> GetAccountTypeById()
        {

        }
        public Task<AccountType> GetAccountTypeByName()
        {

        }
        public Task<bool> AddAccountType()
        {

        }

    }
}