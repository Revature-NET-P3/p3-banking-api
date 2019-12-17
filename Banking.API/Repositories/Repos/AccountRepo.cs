
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
        private readonly AppDbContext _context;

        public AccountRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        //save changes method (must be called by every function calling the repo)
        public async Task<bool> SaveChanges()
        {
            await _context.SaveChangesAsync();
            return true;
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
            account.CreateDate = DateTime.Now;
            account.IsClosed = false;
            _context.Add(account);
            _context.SaveChanges();

            // record the transaction and save it the db.
            Transaction newTrans = new Transaction()
            {
                AccountId = account.Id,
                Ammount = account.Balance > 0 ? account.Balance : 0,
                TimeStamp = DateTime.Now,
                TransactionTypeId = 7
            };
            await _context.Transactions.AddAsync(newTrans);

            return account;
        }

        // deposit account method
        public async Task<bool> Deposit(int Id, decimal amount)
        {
            // update account
            var depositAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);

            //check that account is open
            if (depositAccount.IsClosed)
            {
                return false;
            }

            depositAccount.Balance += amount;

            // record the transaction and save it the db.
            Transaction newTrans = new Transaction() 
            {
                AccountId = Id,
                Ammount = amount,
                TimeStamp = DateTime.Now,
                TransactionTypeId = 2,
                AssociatedAccountId = 0
            };
            _context.Transactions.Add(newTrans);
            _context.Update(depositAccount);
            return true;
        }

        // withdraw account method
        public async Task<bool> Withdraw(int Id, decimal amount)
        {
            var withdrawAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);

            //check that account is open
            if(withdrawAccount.IsClosed)
            {
                return false;
            }

            withdrawAccount.Balance -= amount;
            // record the transaction and save it the db.
            Transaction newTrans = new Transaction()
            {
                AccountId = Id,
                Ammount = amount,
                TimeStamp = DateTime.Now,
                TransactionTypeId = 1,
                AssociatedAccountId = 0
            };
            _context.Transactions.Add(newTrans);
            _context.Update(withdrawAccount);
            return true;
        }

        // withdraw account method
        public async Task<bool> Overdraft(int Id, decimal amount)
        {
            var withdrawAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            // record the transaction and save it the db.
            Transaction newTrans = new Transaction()
            {
                AccountId = Id,
                Ammount = amount,
                TimeStamp = DateTime.Now,
                TransactionTypeId = 5,
            };
            _context.Transactions.Add(newTrans);
            return true;
        }

        // method to transfer between accounts
        public async Task<bool> TransferBetweenAccounts(int Id, decimal fromAmount, int toAccId, decimal toAmount)
        {
            var transferAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);
            var accountTo = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == toAccId);

            //check that account is open
            if (transferAccount.IsClosed || accountTo.IsClosed)
            {
                return false;
            }

            transferAccount.Balance -= fromAmount;
            accountTo.Balance += toAmount;

            // record the transaction from account.
            Transaction newTrans = new Transaction()
            {
                AccountId = Id,
                Ammount = fromAmount,
                TimeStamp = DateTime.Now,
                TransactionTypeId = 3,
                AssociatedAccountId = toAccId
            };

            // record the transaction to account.
            Transaction newTrans1 = new Transaction()
            {
                AccountId = toAccId,
                Ammount = toAmount,
                TimeStamp = DateTime.Now,
                TransactionTypeId = 4,
                AssociatedAccountId = Id
            };
            _context.Transactions.Add(newTrans);
            _context.Transactions.Add(newTrans1);
            _context.Update(transferAccount);
            _context.Update(accountTo);
            return true;
        }

        // method to pay loan
        public async Task<bool> PayLoan(int Id, decimal amount)
        {
            var loanAccount = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == Id);

            //check that account is open
            if (loanAccount.IsClosed)
            {
                return false;
            }


            loanAccount.Balance -= amount;
            _context.Update(loanAccount);
            // record the transaction and save it the db.
            Transaction newTrans = new Transaction()
            {
                AccountId = loanAccount.Id,
                TimeStamp = DateTime.Now,
                Ammount = amount,
                TransactionTypeId = 6,
            };
            await _context.Transactions.AddAsync(newTrans);

            return true;
        }

        // if account id exists and not null, then close account
        public async Task<bool> CloseAccount(int Id)
        {
            var accToClose = await _context.Accounts.Where(e => e.Id == Id).SingleOrDefaultAsync();
            if (accToClose != null)
            {
                accToClose.IsClosed = true;
                _context.Update(accToClose);

                // record the transaction and save it the db.
                Transaction newTrans = new Transaction()
                {
                    AccountId = accToClose.Id,
                    TimeStamp = DateTime.Now,
                    TransactionTypeId = 8,
                };
                await _context.Transactions.AddAsync(newTrans);
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
            var accDetails = await _context.Accounts.Where(e => e.Id == Id).SingleOrDefaultAsync();
            return accDetails;
        }

        // return the list of transactions for a particluar account ID.
        public async Task<IEnumerable<Transaction>> GetTransactionDetailsByAccountID(int Id)
        {
            var transactionDetails = await _context.Transactions.Where(e => e.AccountId == Id).ToListAsync();
            return transactionDetails;
        }
    }
}