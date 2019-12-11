using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Banking.API.Models;
//TODO: Add import for repo

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferablesController : ControllerBase
    {
        // private readonly IAccount _repo; //access to account
        //access to 

        //public TransferablesController(IAccount repo) //dependency injection of repo
        //{
        //    _repo = repo;

        //}

        private static List<Account> accountList = new List<Account>()
            {
                new Account
                {
                    Id = 10,
                    AccountTypeId = 2,
                    Balance = 15.50M,
                    UserId = 60,
                    CreateDate = DateTime.Today
                },
                new Account
                {
                    Id = 20,
                    AccountTypeId = 2,
                    Balance = 15.50M,
                    UserId = 60,
                    CreateDate = DateTime.Today
                },
                new Account
                {
                     Id = 30,
                    AccountTypeId = 2,
                    Balance = 15.50M,
                    UserId = 60,
                    CreateDate = DateTime.Today
                }
            };

        // GET: api/Transferables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Get() //For Testing API
        {
       

            return accountList;
        }

        // GET: api/Transferables/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }


        // POST: api/Transferables
        /// <summary>
        /// method creates/opens a new bank account. Stores it in the accounts table. 
        /// </summary>
        /// <param name="newAccount">new Account object to create and store</param>
        [HttpPost]
        public void Post([FromBody] Account newAccount)
        {

            accountList.Add(newAccount);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="amount">the amount to deposit</param>

        // PUT: api/Transferables/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
