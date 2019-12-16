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
        [HttpPost]
        public async Task<ActionResult> OpenLoan(Account acct)
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

        //put
        [HttpPut]
        public async Task<ActionResult> ProcessLoanPayment(int id, int amount)
        {
            if (amount <= 0) //make sure payment amount is positive
            {
                _logger?.LogWarning(string.Format("PUT request failed, Amount passed is less than or equal to 0.  Account with ID: {0}", id));
                return StatusCode(400);
            }

            var acct = await _repo.GetTransactionDetailsByAccountID(id);

            if(acct == null)
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

        //put
        [HttpPut]
        public async Task<ActionResult> CloseLoan(int id)
        {
            var acct = await _repo.GetTransactionDetailsByAccountID(id);

            if (acct == null)
            {
                _logger?.LogWarning(string.Format("PUT request failed, Account not found.  Account with ID: {0}", id));
                return NotFound(id);
            }
            else
            {
                await _repo.CloseAccount(id);
                return NoContent();
            }
               
        }

    }
}