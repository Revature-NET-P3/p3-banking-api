using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Banking.API.Controllers
{
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
        [HttpPost("open/{acct}")]
        public async Task<ActionResult> OpenLoan([FromBody]Account acct)
        {
            try
            {
                if (acct.AccountTypeId != 3) //Warn: this breaks if database changes type ids
                {
                    _logger?.LogWarning(string.Format("PUT request failed, Account is not a loan. "));
                    return StatusCode(400);
                }
                else
                {
                    await _repo.OpenAccount(acct);
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                await _repo.OpenAccount(acct);
                await _repo.SaveChanges();
                return NoContent();
            }
        }

        //put
        [HttpPut("payLoan/{id}/{amount}")]
        public async Task<ActionResult> ProcessLoanPayment(int id, int amount)
        {
            try
            {
                if (amount <= 0) //make sure payment amount is positive
                {
                    _logger?.LogWarning(string.Format("PUT request failed, Amount passed is less than or equal to 0.  Account with ID: {0}", id));
                    return StatusCode(400);
                }

                var acct = await _repo.GetTransactionDetailsByAccountID(id);

                if (acct == null)
                {
                    _logger?.LogWarning(string.Format("PUT request failed, Account not found.  Account with ID: {0}", id));
                    return NotFound(id);
                }
                else
                {
                    await _repo.PayLoan(id, amount);
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                await _repo.PayLoan(id, amount);
                await _repo.SaveChanges();
                return NoContent();
            }
        }

        //Delete
        [HttpDelete("close/{id}")]
        public async Task<ActionResult> CloseLoan(int id)
        {
            try
            {
                var acct = await _repo.GetTransactionDetailsByAccountID(id);

                if (acct == null)
                {
                    _logger?.LogWarning(string.Format("Delete request failed, Account not found.  Account with ID: {0}", id));
                    return NotFound(id);
                }
                else
                {
                    await _repo.CloseAccount(id);
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                await _repo.CloseAccount(id);
                await _repo.SaveChanges();
                return NoContent();
            }
        }
    }
}