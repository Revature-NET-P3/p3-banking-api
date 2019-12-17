using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using Banking.API.Models;
using Banking.API.Repositories.Interfaces;

namespace Banking.API.Controllers
{
    [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoanAccountController : ControllerBase
    {
        private readonly IAccountRepo _repo;
        private readonly ILogger _logger;

        public LoanAccountController(IAccountRepo newRepo, ILogger<LoanAccountController> logger)
        {
            _repo = newRepo;
            _logger = logger;
        }

        //Post  api/LoanAccount
        [HttpPost("open")]
        public async Task<ActionResult> OpenLoan([FromBody]Account acct)
        {
            try
            {
                if (acct.AccountTypeId != 3) //Warn: this breaks if database changes type ids
                {
                    _logger?.LogWarning(string.Format("LoanAccountController POST request failed, Account is not a loan. "));
                    return StatusCode(400);
                }
                else
                {
                    await _repo.OpenAccount(acct);
                    await _repo.SaveChanges();
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error in LoanAccountController!");
                return StatusCode(500);
            }
        }

        //put
        [HttpPut("payLoan/{id}/{amount}")]
        public async Task<ActionResult> ProcessLoanPayment(int id, decimal amount)
        {
            try
            {
                if (amount <= 0) //make sure payment amount is positive
                {
                    _logger?.LogWarning(string.Format("LoanAccountController PUT request failed, Amount passed is less than or equal to 0.  Account with ID: {0}", id));
                    return StatusCode(400);
                }

                var acct = await _repo.GetAccountDetailsByAccountID(id);

                if (acct == null)
                {
                    _logger?.LogWarning(string.Format("LoanAccountController PUT request failed, Account not found.  Account with ID: {0}", id));
                    return NotFound(id);
                }
                else
                {
                    if (!await _repo.PayLoan(id, amount))
                    {
                        _logger?.LogWarning(string.Format("LoanAccountController PUT request failed, Account not is already closed.  Account with ID: {0}", id));
                        return NotFound(id);
                    }
                   
                    await _repo.SaveChanges();
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error in LoanAccountController!");
                return StatusCode(500);
            }
        }

        //Delete
        [HttpDelete("close/{id}")]
        public async Task<ActionResult> CloseLoan(int id)
        {
            try
            {
                Account acct = await _repo.GetAccountDetailsByAccountID(id);

                if (acct == null)
                {
                    _logger?.LogWarning(string.Format("LoanAccountController DELETE request failed, Account not found.  Account with ID: {0}", id));
                    return NotFound(id);
                }
                else
                {
                    if(acct.Balance > 0)
                    {
                        _logger?.LogWarning(string.Format("LoanAccountController DELETE request failed, Account not empty.  Account with ID: {0}", id));
                        return StatusCode(400);
                    }
                    else
                    {
                        await _repo.CloseAccount(id);
                        await _repo.SaveChanges();
                        return NoContent();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error in LoanAccountController!");
                return StatusCode(500);
            }
        }
    }
}