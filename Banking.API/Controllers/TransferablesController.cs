using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using Banking.API.Repositories.Repos;
using Microsoft.AspNetCore.Cors;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("DefaultPolicy")]
    public class TransferablesController : ControllerBase
    {
        private readonly IAccountRepo _repoAccount; //access to account
        private readonly IAccountTypeRepo _repoAccountType;
        private readonly ILogger<TransferablesController> _logger;

        public TransferablesController(IAccountRepo repoAccount, IAccountTypeRepo repoType, ILogger<TransferablesController> logger) //TODO: add dependency injection of repo
        {
            _repoAccountType = repoType;
            _repoAccount = repoAccount;
            _logger = logger;
        }


        // POST: api/Transferables
        /// <summary>
        /// method creates/opens a new bank account. Stores it in the accounts table. 
        /// </summary>
        /// <param name="newAccount">new Account object to create and store</param>
        [HttpPost]
        public async Task<ActionResult<Account>> Post([FromBody] Account newAccount)
        {
            _logger?.LogInformation(string.Format("Attempting to Create a new into account with id"));

            //TODO: add logic to create/store account using repo
            try
            {
                //accountList.Add(newAccount);
                await _repoAccount.OpenAccount(newAccount);
                await _repoAccount.SaveChanges();

                return CreatedAtAction("Post", new { id = newAccount.Id }, newAccount);

               // return Ok();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected Error in Post new account");
                return StatusCode(500, e);
            }
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


                acctFound = await _repoAccount.GetAccountDetailsByAccountID(id);


                if (acctFound == null)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, No Account found with ID: {0}", id.ToString()));
                    return NotFound(id);
                }

                await _repoAccount.Deposit(acctFound.Id, amount); //TODO: call deposit repo
                _logger?.LogInformation("PUT Success deposited into account with ID: {0} Amount: {2}", id.ToString(), amount.ToString());
                await _repoAccount.SaveChanges();

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

                acctFound = await _repoAccount.GetAccountDetailsByAccountID(id);

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
                        decimal overdraft = 0;
                        AccountType acctType = await _repoAccountType.GetAccountTypeById(acctFound.AccountTypeId); //need account type for interset rate

                        decimal totalAmount = CalculatePenalty(acctFound.Balance, amount, acctType.InterestRate, ref overdraft);

                        // acctFound.Balance -= totalAmount;
                        await _repoAccount.Withdraw(acctFound.Id, totalAmount); //TODO: possible problem pass in totalAmount = amount + overdraftPenalty
                        await _repoAccount.Overdraft(id, overdraft);
                        _logger?.LogInformation("PUT Success withdrew from account but with overdraft, account ID: {0} totalAmount: {1}", id.ToString(), totalAmount.ToString());
                        await _repoAccount.SaveChanges();

                        return NoContent();
                    }
                }

                //no overdraft
                //acctFound.Balance -= amount;
                await _repoAccount.Withdraw(acctFound.Id, amount); //TODO: call withdraw repo
                _logger?.LogInformation("PUT Success withdrew from account with ID: {0}", id.ToString());
                await _repoAccount.SaveChanges();

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
        /// <param name="idFrom">The id of account from where user will transfer from</param>
        /// <param name="idTo">The id of destination account where user wants to transfer into</param>
        /// <param name="amount">the amount to withdraw from account to transfer from</param>
        [HttpPut("transfer/{idFrom}/{idTo}/{amount}")]
        public async Task<ActionResult> Transfer(int idFrom, int idTo, decimal amount)
        {
            _logger?.LogInformation(string.Format("starting transfer from account ID: {0} into account with id: {1}", idFrom.ToString(), idTo.ToString()));

            try
            {
                //TODO: Add logic to get account with specific id
                Account acctFoundFrom = null;
                Account acctFoundTo = null;
                AccountType acctType = null;
                if (amount < 0) //make sure transfer amount is positive
                {
                    _logger?.LogWarning(string.Format("PUT request failed, Amount passed is less than 0.  From Account with ID: {0}", idFrom.ToString()));
                    return StatusCode(400);
                }

                //check to see account with id exist
                acctFoundFrom = await _repoAccount.GetAccountDetailsByAccountID(idFrom);
                acctFoundTo = await _repoAccount.GetAccountDetailsByAccountID(idTo);

                if (acctFoundFrom == null || acctFoundTo == null)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, No Account found with ID: {0} or To ID: {1}", idFrom.ToString(), idTo.ToString()));
                    return NotFound(idFrom);
                }

                //check if withdraw from origin account will cause in an overdraft
                if ((acctFoundFrom.Balance - amount) < 0)
                {
                    //check if it is checking account return an error
                    if (acctFoundFrom.AccountTypeId == 1) //TODO: 1 hard coded to represent checking account replace with actual AccountType check
                    {
                        _logger?.LogWarning(string.Format("PUT request failed, transfer would cause overdraft from Checking Account with ID: {0}", idFrom.ToString()));
                        return StatusCode(400);
                    }
                    else//if account is business account do penalty calculation
                    {
                        acctType = await _repoAccountType.GetAccountTypeById(acctFoundFrom.AccountTypeId); //need account type for interset rate
                        decimal overdraft = 0;

                        decimal totalAmount = CalculatePenalty(acctFoundFrom.Balance, amount, acctType.InterestRate, ref overdraft);

                        //acctFoundFrom.Balance -= totalAmount; 
                        //acctFoundTo.Balance += amount;
                        await _repoAccount.TransferBetweenAccounts(acctFoundFrom.Id, totalAmount, acctFoundTo.Id, amount); //TODO: add transfer repo
                        await _repoAccount.Overdraft(idFrom, overdraft);

                        _logger?.LogInformation("PUT Success, transfer FromAccount ID: {0} ToAccount ID: {1} Amount: {2} TotalAmount {3}", idFrom.ToString(), idTo.ToString(), amount.ToString(), totalAmount.ToString());
                        await _repoAccount.SaveChanges();

                        return NoContent();
                    }
                }

                //acctFoundFrom.Balance -= amount; //TODO: add transfer repo
                // acctFoundTo.Balance += amount;
                await _repoAccount.TransferBetweenAccounts(acctFoundFrom.Id, amount, acctFoundTo.Id, amount); //TODO: add transfer repo

                _logger?.LogInformation("PUT Success, transfer FromAccount ID: {0} ToAccount ID: {1} Amount: {2}", idFrom.ToString(),idTo.ToString(), amount.ToString());
                await _repoAccount.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected Error in deposit of account with ID: {0}", idFrom.ToString());
                return StatusCode(500, e);
            }
        }

        // DELETE: api/Transferables
        /// <summary>
        /// Close the account with a specific id, changes flag is IsClose to false
        /// </summary>
        /// <param name="id">The id of the account you wish to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Close(int id)
        {
            _logger?.LogInformation(string.Format("Attempting to Close account with id: {0}", id.ToString()));

            try
            {
                //TODO: Add Logic to find account and update its balance
                Account acctFound = null;

                acctFound = await _repoAccount.GetAccountDetailsByAccountID(id);

                if (acctFound == null) //check if account exist
                {
                    _logger?.LogWarning(string.Format("DELETE request failed, No Account found with ID: {0}", id.ToString()));
                    return NotFound(id);
                }
                if (acctFound.Balance != 0) //make sure account has no funds and also has no overdraft
                {
                    _logger?.LogWarning(string.Format("DELETE request failed, Balance is not 0. Account with ID: {0}", id.ToString()));
                    return StatusCode(400);
                }

                //TODO: call deposit repo to change flag to is closed
                //accountList.Remove(acctFound);
                await _repoAccount.CloseAccount(acctFound.Id);
                _logger?.LogInformation("DELETE Success Closed account with ID: {0}", id.ToString());
                await _repoAccount.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Unexpected Error in Delete account with ID: {0}", id.ToString());
                return StatusCode(500, e);
            }
        }

        private decimal CalculatePenalty(decimal balance, decimal amount, decimal interestRate, ref decimal overdraftAmount)
        {
            decimal totalPenalty;

            if (balance <= 0) //true means the business account already been overdrafted or no funds
            {
                //penalty on the whole withdraw amount
                overdraftAmount = amount * interestRate; //TODO: Hard Coded interest rate of, replace with AccountType interest rate
                totalPenalty = overdraftAmount + amount;
            }
            else //first time account overdrafts
            {
                //user has funds in account, penalty only on the amount that user overdrafted on
                overdraftAmount = (amount - balance) * interestRate; //TODO: Hard Coded interest rate of, replace with AccountType interest rate
                totalPenalty = (amount - balance) + overdraftAmount + balance; //total should reflect negative balance
            }

            return totalPenalty;
        }
    }
}
