using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Repos
{
    public class AccountRepo : IAccountRepo
    {
        private List<Account> _accounts;

        // create a mock data for test purposes.
        public AccountRepo()
        {
            _accounts = new List<Account>()
            {
                new Account() { Id = 1, UserId = 10, AccountTypeId = 3, Balance = 200, CreatedDate = "12/10/2019" },
                new Account() { Id = 2, UserId = 20, AccountTypeId = 1, Balance = 300, CreatedDate = "12/12/2019" },
                new Account() { Id = 3, UserId = 30, AccountTypeId = 2, Balance = 500, CreatedDate = "12/15/2019" }
            };
        }

        //get a single account
        public async Account GetAccount(int Id)
        {
            return await _accounts.FirstOrDefaultAsync(e => e.Id == Id);
        }

        //get all accounts
        public async IEnumerable<Account> GetAllAccount()
        {
            return await _accounts();
        }

        //get list of accounts belonging to a user.
        public async Task<List<LoanAccount>> GetAccounts(int UserId)
        {
            var account = await _accounts.Where(e => e.UserID == UserId).ToListAsync();
            return account;
        }

        //add a new account
        public async Task<bool> AddAccount(Account account)
        {
            account.Id = _accounts.FirstOrDefaultAsync(e => e.Id == account.Id);
            await _accounts.Add(account);
            return account;
        }

        //delete an account by id
        //public async Account DeleteAccount(int Id)
        //{
        //    Account account = _accounts.FirstorDefaultAsync(e => e.Id == Id);
        //    if (account != null)
        //    {
        //        _accounts.Remove(account);
        //    }
        //    return await account;
        //}

        //update account
        // will determin if this is needed.
        //public async Task<bool> UpdateAccount(Account accountChanges)
        //{
        //    account.Id = _accounts.FirstOrDefaultAsync(e => e.Id == accountChanges.Id);
        //    _accounts.Update(account);
        //    return await account;
        //}
    }
}
