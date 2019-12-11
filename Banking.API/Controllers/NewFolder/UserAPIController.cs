using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Banking.API.Repositories;
using Banking.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.API.Controllers.NewFolder
{
    // [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : Controller
    {
              
            private readonly UserRepo _context;

            public UserAPIController(UserRepo context)
            {
                _context = context;
            }

            //// GET: api/User
            //[HttpGet]
            //public async Task<ActionResult<IEnumerable<User>>> GetUser()
            //{
            //    return await _context.SelectAll();
            //}

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

            //[HttpGet("ByUser/{Username}")]
            //public async Task<ActionResult<User>> GetCustomerByUser(string Username)
            //{
            //    var customer = await _context.SelectByUser(Username);

            //    if (customer == null)
            //    {
            //        return NotFound();
            //    }

            //    return customer;
            //}

            // PUT: api/User/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for
            // more details see https://aka.ms/RazorPagesCRUD.
            [HttpPut("{id}")]
            public async Task<IActionResult> PutCustomer(int id, User user)
            {
                if (id != user.Id)
                {
                    return BadRequest();
                }


                try
                {
                    await _context.UpdateUser(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            // POST: api/User
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for
            // more details see https://aka.ms/RazorPagesCRUD.
            [HttpPost]
            public async Task<ActionResult<User>> PostCustomer(User user)
            {
           
                await _context.CreateUser(user);


                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }

            //// DELETE: api/User/5
            //[HttpDelete("{id}")]
            //public async Task<ActionResult<User>> DeleteCustomer(int id)
            //{
            //    var user = await _context.SelectById(id);
            //    if (user == null)
            //    {
            //        return NotFound();
            //    }

            //    await _context.Remove(user);

            //    return user;
            //}

            //GET: api/

            private bool UserExists(int id)
            {
                return _context.UserExists(id);
            }
        
    }


    
}
