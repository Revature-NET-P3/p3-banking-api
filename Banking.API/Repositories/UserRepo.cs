using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Repositories.Interfaces;
using Banking.API.Models;

namespace Banking.API.Repositories
{
    public class UserRepo : IUserRepo
    {
        private AppDbContext _context;

        public UserRepo(AppDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<bool> CreateUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> ViewById(int id)
        {
            User gotUser = await _context.User.FirstOrDefaultAsync(o => o.Id == id);
            return gotUser;
        }
        public async Task<bool> UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> VerifyLogin(string username, string passhash)
        {
            User user = await _context.FirstOrDefaultAsync(o => o.Username == username);
            if(user.PasswordHash == passhash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
