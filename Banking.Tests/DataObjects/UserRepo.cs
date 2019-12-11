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
        List<User> usersList;

        public UserRepo()
        {
            usersList = new List<User>()
            {
                new User{ Id = 1 , Username = "Swag0" , Email = "null" , PasswordHash = "HashBrown" },
                new User{ Id = 2 , Username = "Swag1" , Email = "null" , PasswordHash = "HashBrown" },
                new User{ Id = 3 , Username = "Swag2" , Email = "null" , PasswordHash = "HashBrown" },
                new User{ Id = 4 , Username = "Swag3" , Email = "null" , PasswordHash = "HashBrown" }
            };
        }

        public async Task<bool> CreateUser(User user)
        {
            User gotUsername = usersList.FirstOrDefault(o => o.Username == user.Username);
            User gotUserid = usersList.FirstOrDefault(o => o.Id == user.Id);
            if (!(gotUsername is null)) { throw new Exception("That username already exists!"); }
            else if (!(gotUsername is null)) { throw new Exception("That UserId already exists! Id is identity, do not pass value."); }
            else
            {
                await Task.Delay(10);
                usersList.Add(user);
                return true;
            }
        }
        public async Task<User> ViewById(int id)
        {
            User gotUserid = usersList.FirstOrDefault(o => o.Id == id);
            if (gotUserid is null) { throw new Exception("No user exists with that id."); }
            await Task.Delay(10);
            return (User)gotUserid;
        }
        public async Task<bool> UpdateUser(User user)
        {
            User getUser = usersList.FirstOrDefault(o => o.Id == user.Id);
            if(getUser is null) { throw new Exception("That user does not currently exist."); }
            await Task.Delay(10);
            usersList.Remove(getUser);
            usersList.Add(user);            
            return true;
        }
        public async Task<bool> VerifyLogin(string username, string passhash)
        {
            User user = usersList.FirstOrDefault(o => o.Username == username);
            if (user is null) { throw new Exception("There is no user with that username."); }
            await Task.Delay(10);
            if (user.PasswordHash == passhash)
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
