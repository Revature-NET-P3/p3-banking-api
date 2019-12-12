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
        // use the DbContext 
        private AppDbContext _context;

        public AccountRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        //get a single account
        public async Task<Account> GetAccount(int Id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            return account;
        }

        //get list of accounts belonging to a user.
        public Task<List<Account>> GetUserAccounts(int UserId)
        {
            var account = _context.Accounts.Where(e => e.UserId == UserId).ToListAsync();
            // return _accounts.FirstOrDefault(e => e.Id == Id); for mock data
            return account;
        }


        //add a new account
        public async Task<Account> OpenAccount(Account account)
        {
            var accounts = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == account.Id);
            _context.Add(account);
            _context.SaveChanges();
            return accounts;
        }

        // deposit account method
        public async Task<bool> Deposit(int Id, decimal amount)
        {
            var depositAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            depositAccount.Balance += amount;
            _context.Update(depositAccount);
            return true;
        }

        // withdraw account method
        public async Task<bool> Withdraw(int Id, decimal amount)
        {
            var withdrawAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            withdrawAccount.Balance -= amount;
            return true;
        }

        // method to transfer between accounts
        public async Task<bool> TransferBetweenAccounts(int Id, decimal fromAmount, int toAccId, decimal toAmount)
        {
            var transferAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            var accountTo = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == toAccId);
            transferAccount.Balance -= fromAmount;
            accountTo.Balance += toAmount;
            _context.Update(transferAccount);
            _context.Update(accountTo);
            await _context.SaveChangesAsync();
            return true;
        }

        // method to pay loan
        public async Task<bool> PayLoan(int Id, decimal amount)
        {
            var loanAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            loanAccount.Balance -= amount;
            _context.Update(loanAccount);
            await _context.SaveChangesAsync();
            return true;
        }

        // if account id exists and not null, then close account
        public async Task<bool> CloseAccount(int Id)
        {
            var accToClose = await _context.Accounts.Where(e => e.Id == Id).SingleAsync();
            if (accToClose != null)
            {
                accToClose.IsClosed = true;
                _context.Update(accToClose);
                await _context.SaveChangesAsync();
            }
            return false;
        }

        // if user id exists, and not null, return all user accounts. Else return false.
        public async Task<IEnumerable<Account>> GetAllAccountsByUserId(int UserId)
        {
            var account = await _context.Accounts.Where(e => e.UserId == UserId).ToListAsync();
            return account;
        }

        // check if user id and account type id are not null and compare if account exits.
        // if parameters are empty, return false.
        public async Task<IEnumerable<Account>> GetAllAccountsByUserIdAndAccountType(int UserId, int AccountTypeId)
        {
            var accByType = await _context.Accounts.Where(e => e.UserId == UserId && e.AccountTypeId == AccountTypeId).ToListAsync();
            return accByType;
        }

        // retruns a single account based on the account ID.
        public async Task<Account> GetAccountDetailsByAccountID(int Id)
        {
            var accDetails = await _context.Accounts.Where(e => e.Id == Id).SingleAsync();
            return accDetails;
        }

        // return the list of transactions for a particluar account ID.
        public async Task<IEnumerable<Transaction>> GetTransactionDetailsByAccountID(int Id)
        {
            var transactionDetails = await _context.Transactions.Where(e => e.Id == Id).ToListAsync();
            return transactionDetails;
        }
    }
}
