using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        Account GetAccount(int Id);
        Account GetAccounts(int UserId);
        IEnumerable<Account> GetAllAccount();
        Account AddAccount(Account account);
        //Account UpdateAccount(Account accountChanges);
        //Account DeleteAccount(int Id);
    }
}
