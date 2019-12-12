using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Banking.API.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    public class UserAPIController : ControllerBase
    {

        private readonly UserRepo _context;

        public UserAPIController(UserRepo context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateUser(User user)
        {
            await _context.CreateUser(user);
            return true;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.ViewById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> UpdateUser(User user)
        {
            await _context.UpdateUser(user);
            return true;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> VerifyLogin(string username, string passhash)
        {
            await _context.VerifyLogin(username, passhash);
            return true;
        }


    }
}
