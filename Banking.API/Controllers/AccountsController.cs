using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Banking.API.Models;
using Banking.API.Repositories.Repos;
using Banking.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace Banking.API.Controllers
{
    [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // TODO: Add injection for repository when ready.
        readonly ILogger<AccountsController> _logger;
        readonly IAccountRepo _repo;

        public AccountsController(IAccountRepo newRepo, ILogger<AccountsController> newLogger)
        {
            _logger = newLogger;
            _repo = newRepo;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountsByUserID(int id)
        {
            try
            {
                IEnumerable<Account> result = null;
                _logger?.LogInformation(string.Format("Start GetAllAccountsByUserID: {0}", id.ToString()));
                result = await _repo?.GetAllAccountsByUserId(id) ?? null;

                // Check if returned list has any elements.
                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response on empty list.
                    _logger?.LogWarning(string.Format("No Accounts found for UserID: {0}", id.ToString()));
                    return NotFound(id);
                }

                // Return list of accounts on successful find.
                _logger?.LogInformation(string.Format("GetAllAccountsByUserID: {0} Succeeded.", id.ToString()));
                return Ok(result.ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserID!");
                return StatusCode(StatusCodes.Status500InternalServerError, WTF);
            }
        }

        // GET: api/Accounts/5/1
        [HttpGet("{id}/{typeid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountsByUserIDAndTypeID(int id, int typeid)
        {
            try
            {
                IEnumerable<Account> result = null;
                _logger?.LogInformation(string.Format("Start GetAllAccountsByUserID: {0}, Filtered by TypeID: {1}", id.ToString(), typeid.ToString()));
                result = await _repo?.GetAllAccountsByUserIdAndAccountType(id, typeid) ?? null;

                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response on empty list.
                    _logger?.LogWarning(string.Format("No Accounts found for UserID: {0}, with Filter by TypeID: {1}", id.ToString(), typeid.ToString()));
                    return NotFound(id);
                }

                // Return list of accounts on successful find.
                _logger?.LogInformation(string.Format("GetAllAccountsByUserID: {0}, Filtered by TypeID: {1} Succeeded", id.ToString(), typeid.ToString()));
                return Ok(result.ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserIDAndTypeID!");
                return StatusCode(StatusCodes.Status500InternalServerError, WTF);
            }
        }

        // GET: api/Accounts/details/4
        [HttpGet("details/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Account>> GetAccountDetailsByAccountID(int id)
        {
            try
            {
                Account result = null;
                _logger?.LogInformation(string.Format("Start GetAccountDetailsByAccountID: {0}", id.ToString()));
                result = await _repo?.GetAccountDetailsByAccountID(id) ?? null;
                
                // Check if return object was null.
                if (result == null)
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} details not found!", id.ToString()));
                    return NotFound(id);
                }

                // Return account object found.
                _logger?.LogInformation(string.Format("GetAccountDetailsByAccountID: {0} Succeeded.", id.ToString()));

                return Ok(result);
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAccountDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError, WTF);
            }
        }

        // GET: api/Accounts/transactions/4
        [HttpGet("transactions/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionDetailsByAccountID(int id)
        {
            try
            {
                IEnumerable<Transaction> result = null;
                _logger?.LogInformation(string.Format("Start GetTransactionDetailsByAccountID: {0}", id.ToString()));
                result = await _repo?.GetTransactionDetailsByAccountID(id) ?? null;

                // Check if return object was null.
                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} transaction details not found!", id.ToString()));
                    return NotFound(id);
                }

                // Return list of transactions found.
                _logger?.LogInformation(string.Format("GetTransactionDetailsByAccountID: {0} Succeeded.", id.ToString()));
                return Ok(result.ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetTransactionDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError, WTF);
            }
        }
    }
}