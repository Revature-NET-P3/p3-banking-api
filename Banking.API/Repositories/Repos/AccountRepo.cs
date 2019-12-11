using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        //get list of accounts belonging to a user.
        public Task<List<Account>> GetUserAccounts(int UserId)
        {
            var account = _context.Accounts.Where(e => e.UserId == UserId).ToListAsync();
            // return _accounts.FirstOrDefault(e => e.Id == Id); for mock data
            return account;
        }


        //add a new account
        public Account OpenAccount(Account account)
        {
            var accounts = _context.Accounts.FirstOrDefault(e => e.Id == account.Id);
            _context.Add(account);
            _context.SaveChanges();
            return accounts;
        }

        // deposit account method
        public async Task<bool> Deposit(int Id, decimal amount)
        {
            var depositAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            depositAccount.Balance = depositAccount.Balance + amount;
            _context.Update(depositAccount);
            return true;
        }

        // withdraw account method
        public async Task<bool> Withdraw(int Id, decimal amount)
        {
            var withdrawAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            withdrawAccount.Balance = withdrawAccount.Balance - amount;
            return true;
        }

        // method to transfer between accounts
        public async Task<bool> TransferBetweenAccounts(int Id, decimal amount, int toAccId)
        {
            var transferAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            var accountTo = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == toAccId);
            transferAccount.Balance = transferAccount.Balance - amount;
            accountTo.Balance = accountTo.Balance + amount;
            _context.Update(transferAccount);
            _context.Update(accountTo);
            await _context.SaveChangesAsync();
            return true;
        }

        // method to pay loan
        public async Task<bool> PayLoan(int Id, decimal amount)
        {
            var loanAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            loanAccount.Balance = loanAccount.Balance - amount;
            _context.Update(loanAccount);
            await _context.SaveChangesAsync();
            return true;
        }

        public Account CloseAccount(int Id)
        {
            throw new NotImplementedException();
        }

        public Account GetAllAccountsByUserId(int UserId)
        {
            var account = _context.Accounts.Where(e => e.UserId == UserId).ToList();
            return account;
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
