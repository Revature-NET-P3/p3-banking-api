/*
using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Repos
{
    public class AccountRepo : IAccountRepo
    {
        //private List<Account> _accounts;

        // create a mock data for test purposes.
        //public AccountRepo()
        //{
        //    _accounts = new List<Account>()
        //    {
        //        new Account() { Id = 1, UserId = 10, AccountTypeId = 3, Balance = 200, CreateDate = DateTime.Now },
        //        new Account() { Id = 2, UserId = 20, AccountTypeId = 1, Balance = 300, CreateDate = DateTime.Today},
        //        new Account() { Id = 3, UserId = 30, AccountTypeId = 2, Balance = 500, CreateDate = DateTime.Today },
        //        new Account() { Id = 4, UserId = 30, AccountTypeId = 4, Balance = 500, CreateDate = DateTime.Now }
        //    };
        //}

        // use the DbContext 
        private AppDbContext _context;

        public AccountRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        //get a single account
        public Task<Account> GetAccount(int Id)
        {
            var account = _context.Accounts.FirstOrDefault(e => e.Id == Id);
            return account;
            // return _accounts.FirstOrDefault(e => e.Id == Id); for mock data
        }

        //get list of accounts belonging to a user.
        public Task<List<Account>> GetUserAccounts(int UserId)
        {
            var account = _context.Accounts.Where(e => e.UserId == UserId).ToList();
            return account;
        }

        //add a new account
        public Task<bool> AddAccount(Account account)
        {
            var accounts = _context.Accounts.FirstOrDefault(e => e.Id == account.Id);
            _context.Add(account);
            _context.SaveChanges();
            return accounts;
        }

        Account IAccountRepo.GetUserAccounts(int UserId)
        {
            var account = _context.Accounts.Where(e => e.UserId == UserId).ToList();
            return account;
        }

        Account IAccountRepo.AddAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account OpenAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account Deposit(int Id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Account Withdraw(int Id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Account TransferBetweenAccounts(int Id, decimal amount, int toAccId)
        {
            throw new NotImplementedException();
        }

        public Account PayLoan(int Id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Account CloseAccount(int Id)
        {
            throw new NotImplementedException();
        }

        public Account GetAllAccountsByUserId(int UserId)
        {
            throw new NotImplementedException();
        }

        public Account GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountDetailsByAccountID(int Id)
        {
            throw new NotImplementedException();
        }

        public Account GetTransactionDetailsByAccountID(int Id)
        {
            throw new NotImplementedException();
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
*/