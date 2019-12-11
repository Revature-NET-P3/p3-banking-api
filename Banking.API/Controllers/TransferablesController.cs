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
        private readonly ILogger<TransferablesController> _logger;

        public TransferablesController(ILogger<TransferablesController> logger) //dependency injection of repo
        {
            //Object _repo = repo;
            _logger = logger;
        }

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
        public async Task<ActionResult<Account>> Post([FromBody] Account newAccount)
        {
            //TODO: add logic to create/store account using repo
            accountList.Add(newAccount);
            return Ok();
        }

        // PUT: api/Transferables/deposit/5/10.50
        /// <summary>
        /// Takes the id passed, retrieves the correct account and updates the balance in the account
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="amount">the amount to deposit</param>
        [HttpPut("deposit/{id}/{amount}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Deposit(int id, decimal amount)
        {
            _logger?.LogInformation(string.Format("Attempting to deposit into account with id: {0}", id.ToString()));

            try
            {
                //Add Logic to find account and update its balance
                Account acctFound = null;
                if (amount < 0)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, Amount passed is less than 0.  Account with ID: {0}", id.ToString()));
                    return StatusCode(400);
                }

                foreach (var acct in accountList) //for testing
                {
                    if (acct.Id == id)
                    {
                        acctFound = acct;
                    }
                }

                if(acctFound == null)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, No Accounts found with ID: {0}", id.ToString()));
                    return NotFound(id);
                }

                acctFound.Balance += amount;
                _logger?.LogInformation("PUT Success deposited into account with ID: {0}", id.ToString());
                return NoContent();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected Error in deposit of account with ID: {0}", id.ToString());
                return StatusCode(500, e);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
