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
            accountTypes = new List<AccountType>()
            {
                new AccountType{ Id = 1 , Name = "Checking" , InterestRate = .008M },
                new AccountType{ Id = 2 , Name = "Business" , InterestRate = .02M },
                new AccountType{ Id = 3 , Name = "Loan" , InterestRate = .01M },
                new AccountType{ Id = 4 , Name = "Term Deposit" , InterestRate = .01M }
            };
        }

        public async Task<List<AccountType>> GetAccountTypes()
        {
            await Task.Delay(10);
            return accountTypes;
        }
        public async Task<AccountType> GetAccountTypeById(int id)
        {
            await Task.Delay(10);
            return accountTypes.FirstOrDefault(o => o.Id == id);           
        }
        public async Task<AccountType> GetAccountTypeByName(string name)
        {
            await Task.Delay(10);
            return accountTypes.FirstOrDefault(x => x.Name == name);
        }
        public async Task<bool> AddAccountType(AccountType accType)
        {
            await Task.Delay(10);
            accountTypes.Add(accType);
            return true;
        }

    }
}
