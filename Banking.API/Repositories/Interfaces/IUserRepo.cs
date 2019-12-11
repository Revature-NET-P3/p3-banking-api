using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;

namespace Banking.API.Repositories.Interfaces
{
    public interface IUserRepo
    {
        public Task<bool> CreateUser(User user);
        public Task<User> ViewById(int id);
        public Task<bool> UpdateUser(User user);
        public Task<bool> VerifyLogin(string username, string passhash);
    }
}
