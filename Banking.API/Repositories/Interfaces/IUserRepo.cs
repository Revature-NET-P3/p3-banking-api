using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;

namespace Banking.API.Repositories.Interfaces
{
    public interface IUserRepo
    {
        Task<bool> CreateUser(User user);
        Task<User> ViewById(int id);
        Task<bool> UpdateUser(User user);
        Task<bool> VerifyLogin(string username, string passhash);
    }
}
