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
    public class TermCDController : ControllerBase
    {
        const int termDepositId = 4;
        private readonly IAccountRepo _Context;
        private readonly ILogger<TermCDController> _Logger;

        public TermCDController(IAccountRepo ctx, ILogger<TermCDController> logger)
        {
            _Context = ctx;
            _Logger = logger;
        }

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
                    if (ammountToWithdraw < 0 || ammountToWithdraw > input.Balance)
                    {
                        _Logger.LogWarning("Withdraw amount specified invalid TermCDController Withdraw Action!");
                        return BadRequest();
                    }
                    
                    _Logger.LogInformation($"Withdrawing from Term CD {id}.");

                    if (!await _Context.Withdraw(id, ammountToWithdraw))
                    {
                        _Logger.LogWarning($"Cannot withdraw account is closed");
                        return NotFound(id);
                    }

                    await _Context.SaveChanges();

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

        [HttpPut("transfer/{fromID}/{toID}/{ammountToTransfer}")]
        public async Task<IActionResult> Transfer(int fromID, int toID, decimal ammountToTransfer)
        {
            try
            {
                // Get reference account.
                _Logger.LogInformation($"Getting Term CD Account{fromID} for withdrawal transfer.");
                Account fromAccount = await _Context.GetAccountDetailsByAccountID(fromID);
                _Logger.LogInformation($"Getting Account{toID} for deposit transfer.");
                Account toAccount = await _Context.GetAccountDetailsByAccountID(toID);

                // Check account validity.
                if (fromAccount == null)
                {
                    _Logger.LogWarning($"Term CD {fromID} not found.");
                    return NotFound(null);
                }
                if (toAccount == null)
                {
                    _Logger.LogWarning($"Transfer deposit Account {toID} not found.");
                    return NotFound(null);
                }

                // Check if one year has passed.
                if (fromAccount.CreateDate.Subtract(DateTime.Now).TotalDays < -365)
                {
                    if (ammountToTransfer < 0 || ammountToTransfer > fromAccount.Balance)
                    {
                        _Logger.LogWarning("Withdraw amount specified invalid TermCDController Withdraw Action!");
                        return BadRequest();
                    }

                    _Logger.LogInformation($"Transfering from Term CD {fromID} to deposit Account {toID}.");
                    await _Context.TransferBetweenAccounts(fromID, ammountToTransfer, toID, ammountToTransfer);
                    return NoContent();
                }
                else
                {
                    _Logger.LogWarning($"Term CD {fromID} NOT matured.");
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Unexpected Error in TermCDController Withdraw Action!");
                return StatusCode(500, e);
            }
        }

        [HttpPost("open")]
        public async Task<IActionResult> AddTermCD([FromBody]Account addMe)
        {
            try
            {
                if (addMe.AccountTypeId == termDepositId)
                {
                    _Logger.LogInformation($"Creating new TermCD account.");
                    await _Context.OpenAccount(addMe);
                    _Logger.LogInformation($"Created new TermCD account #{addMe.Id}.");
                    return CreatedAtAction("Post", new { id = addMe.Id }, addMe);
                    //return Ok();
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Unexpected Error in TermCDController Withdraw Action!");
                return StatusCode(500, e);
            }
        }
    }
}