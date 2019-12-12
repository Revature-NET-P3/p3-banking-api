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

        public TransferablesController(ILogger<TransferablesController> logger) //TODO: add dependency injection of repo
        {
            //_repo = repo;
            _logger = logger;
        }

        private static List<Account> accountList = new List<Account>() //JUST for testing
            {
                new Account
                {
                    Id = 10,
                    AccountTypeId = 1,
                    Balance = 15.50M,
                    UserId = 60,
                    CreateDate = DateTime.Today
                },
                new Account
                {
                    Id = 20,
                    AccountTypeId = 1,
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
        /// Retrieves account that matches id passed and deposits into the account by increasing the balance
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
                //TODO: Add Logic to find account and update its balance
                Account acctFound = null;
                if (amount < 0) //make sure deposit amount is positive
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
                    _logger?.LogWarning(string.Format("PUT request failed, No Account found with ID: {0}", id.ToString()));
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

        // PUT: api/Transferables/withdraw/5/10.50
        /// <summary>
        /// Retrieves account that matches id passed and withdraw from the account by decreasing the balance
        /// </summary>
        /// <param name="id">The id of the account</param>
        /// <param name="amount">the amount to withdraw from account</param>
        [HttpPut("withdraw/{id}/{amount}")]
        public async Task<ActionResult> Withdraw(int id, decimal amount)
        {
            try
            {
                //TODO: Add Logic to find account and decreaset balance based on amount
                Account acctFound = null;
                if (amount < 0) //make sure withdraw amount is positive
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

                if (acctFound == null)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, No Account found with ID: {0}", id.ToString()));
                    return NotFound(id);
                }


                //check if withdraw causes overdraft, different actions based on account type 
                if((acctFound.Balance - amount) < 0)
                {
                    //check if it is checking account return an error
                    if (acctFound.AccountTypeId == 1) //TODO: 1 hard coded to represent checking account replace with actual AccountType check
                    {
                        _logger?.LogWarning(string.Format("PUT request failed, Withdraw would overdraft Checking Account with ID: {0}", id.ToString()));
                        return StatusCode(400);
                    }
                    else//if business account do penalty calculation
                    {
                        decimal overdraftAmount;
                        if (acctFound.Balance <= 0) //true means the business account already been overdrafted or no funds
                        {
                            //penalty on the whole withdraw amount
                            overdraftAmount = amount * .25M; //TODO: Hard Coded interest rate of, replace with AccountType interest rate
                            acctFound.Balance -= overdraftAmount + amount; //subtract overdraftamount plus the amount so total is still negative
                        }
                        else //first time account overdrafts
                        {
                            //user has funds in account, penalty only on the amount that user overdrafted on
                            overdraftAmount = (acctFound.Balance - amount) * .25M; //TODO: Hard Coded interest rate of, replace with AccountType interest rate
                            acctFound.Balance = (acctFound.Balance - amount) + overdraftAmount; //total should reflect negative balance
                        }
                        _logger?.LogInformation("PUT Success withdrew from account but with overdraft, account ID: {0}", id.ToString());
                        return NoContent();
                    }
                }

                //no overdraft
                acctFound.Balance -= amount;
                _logger?.LogInformation("PUT Success withdrew from account with ID: {0}", id.ToString());
                return NoContent();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected Error in withdraw of account with ID: {0}", id.ToString());
                return StatusCode(500, e);
            }

        }


        // PUT: api/Transferables/transfer/5/9/10.50
        /// <summary>
        /// Retrieves account that matches id passed and withdraw from the account by decreasing the balance
        /// </summary>
        /// <param name="idFrom">The id of account from where we will transfer from</param>
        /// <param name="idTo">The id of destination account we want to transfer into</param>
        /// <param name="amount">the amount to withdraw from account to transfer from</param>
        [HttpPut("transfer/{idFrom}/{idTo}/{amount}")]
        public async Task<ActionResult> Transfer(int idFrom, int idTo, decimal amount)
        {
            _logger?.LogInformation(string.Format("starting transfer from account ID: {0} into account with id: {1}", idFrom.ToString(), idTo.ToString()));

            try
            {
                //TODO: Add Logic to find account and update its balance
                Account acctFoundFrom = null;
                Account acctFoundTo = null;
                if (amount < 0) //make sure transfer amount is positive
                {
                    _logger?.LogWarning(string.Format("PUT request failed, Amount passed is less than 0.  From Account with ID: {0}", idFrom.ToString()));
                    return StatusCode(400);
                }

                foreach (var acct in accountList) //for testing
                {
                    if (acct.Id == idFrom)
                    {
                        acctFoundFrom = acct;
                    }
                    if (acct.Id == idTo)
                    {
                        acctFoundTo = acct;
                    }
                }

                if (acctFoundFrom == null || acctFoundTo == null)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, No Account found with ID: {0} or To ID: {1}", idFrom.ToString(), idTo.ToString()));
                    return NotFound(idFrom);
                }

                acctFoundFrom.Balance -= amount;
                acctFoundTo.Balance += amount;
                _logger?.LogInformation("PUT Success deposited into account with ID: {0}", idTo.ToString());
                return NoContent();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected Error in deposit of account with ID: {0}", idFrom.ToString());
                return StatusCode(500, e);
            }
        }
    }
}
