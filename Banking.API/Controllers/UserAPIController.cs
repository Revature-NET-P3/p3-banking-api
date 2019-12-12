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
    [ApiController]
    public class UserAPIController : ControllerBase
    {

        private readonly UserRepo _context;

        public UserAPIController(UserRepo context)
        {
            _context = context;
        }
        /// <summary>
        /// This method will be work to create user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>New User</returns>
        // POST: api/Createuser
        [HttpPost]
        public async Task<ActionResult<bool>> CreateUser(User user)
        {
            bool result = await _context.CreateUser(user);
            return true;
         


        }
        /// <summary>
        /// This method wil return the user by Id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return User</returns>
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

        /// <summary>
        /// This method will work to update user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Updated User</returns>
        // PUT: api/Updateuser/5
        [HttpPut("Updateuser")]
        public async Task<ActionResult<bool>> UpdateUser(User user)
        {
             await _context.UpdateUser(user);
            return true;

            
        }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="username"></param>
      /// <param name="passhash"></param>
      /// <returns></returns>
        // GET: api/Verifylogin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bool>> VerifyLogin(string username, string passhash)
        {
         
           var result =  await _context.VerifyLogin(username, passhash);
            return result;
                   

        }


    }
}
