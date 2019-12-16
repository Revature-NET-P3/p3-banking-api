using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Banking.API.Repositories.Repos;
using System.Net.Http;
using Banking.API.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermCDController : ControllerBase
    {
        const int termDepositId = 4;
        private readonly IAccountRepo _Context;
        private readonly ILogger<TermCDController> _Logger;

        // TODO: Inject ILogger<TermCDController> object into constructor.
        // TODO: Save injected ILogger object to a private readonly field inside the TermCDController class.
        // TODO: Log functional steps through out controller actions.
        // TODO: Add Exception handling to all action methods.
        public TermCDController(IAccountRepo ctx, ILogger<TermCDController> logger)
        {
            _Context = ctx;
            _Logger = logger;
        }

        // TODO: Change input account to reference account ID#(int).
        // TODO: Update routing to accept both {input} and {ammounttowithdraw}.
        // TODO: Update specified account through IAccountRepo object.
        [HttpPut("withdraw/{id}/{ammountToWithdraw}")]
        public async Task<IActionResult> Withdraw(int id, decimal ammountToWithdraw)
        {
            try
            {
                _Logger.LogInformation($"Getting Term CD {id} for withdrawal.");
                // Get reference account.
                Account input = await _Context.GetAccountDetailsByAccountID(id);
                if (input == null)
                {
                    _Logger.LogWarning($"Term CD {id} not found.");
                    return NotFound(null);
                }

                // Check if one year has passed.
                if (input.CreateDate.Subtract(DateTime.Now).TotalDays < -365)
                {
                    _Logger.LogInformation($"Withdrawing from Term CD {id}.");
                    await _Context.Withdraw(id, ammountToWithdraw);
                    return NoContent();
                }
                else
                {
                    _Logger.LogWarning($"Term CD {id} NOT matured.");
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Unexpected Error in TermCDController Withdraw Action!");
                return StatusCode(500, e);
            }
        }

        // TODO: Change input account to reference account ID#(int).
        // TODO: Change otherInput account to reference account ID#(int).
        // TODO: Update routing to accept both {input} and {ammountToTransfer}.
        // TODO: Update specified account through IAccountRepo object.
        [HttpPut("transfer/{ammountToTransfer}")]
        public async Task<IActionResult> Transfer([FromBody]Account input, /* Commented out for run issues.[FromBody]Account otherInput,*/ decimal ammountToTransfer)
        {
            DateTime compareDate = input.CreateDate;
            compareDate.AddYears(1);

            if (input.AccountTypeId == termDepositId && input.Balance >= ammountToTransfer && compareDate.CompareTo(DateTime.Now) < 0)
            {
                input.Balance -= ammountToTransfer;
                //otherInput.Balance += ammountToTransfer;
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPost("open")]
        public async Task<IActionResult> AddTermCD([FromBody]Account addMe)
        {
            if (addMe.AccountTypeId == termDepositId)
            {
                await _Context.OpenAccount(addMe);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}