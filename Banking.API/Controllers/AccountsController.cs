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
using Microsoft.AspNetCore.Authorization;

namespace Banking.API.Controllers
{
    [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
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
                result = result?.Where(a => !a.IsClosed).ToList() ?? null;

                // Check if returned list has any elements.
                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response on empty list.
                    _logger?.LogWarning(string.Format("No Accounts found for UserID: {0}", id.ToString()));
                    return NotFound(null);
                }

                // Return list of accounts on successful find.
                _logger?.LogInformation(string.Format("GetAllAccountsByUserID: {0} Succeeded.", id.ToString()));
                return Ok(result.ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                result = result?.Where(a => !a.IsClosed).ToList() ?? null;

                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response on empty list.
                    _logger?.LogWarning(string.Format("No Accounts found for UserID: {0}, with Filter by TypeID: {1}", id.ToString(), typeid.ToString()));
                    return NotFound(null);
                }

                // Return list of accounts on successful find.
                _logger?.LogInformation(string.Format("GetAllAccountsByUserID: {0}, Filtered by TypeID: {1} Succeeded", id.ToString(), typeid.ToString()));
                return Ok(result.ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserIDAndTypeID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                    return NotFound(null);
                }

                // Return account object found.
                _logger?.LogInformation(string.Format("GetAccountDetailsByAccountID: {0} Succeeded.", id.ToString()));

                return Ok(result);
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAccountDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                    return NotFound(null);
                }

                // Return list of transactions found.
                _logger?.LogInformation(string.Format("GetTransactionDetailsByAccountID: {0} Succeeded.", id.ToString()));
                return Ok(result.OrderByDescending(tr => tr.TimeStamp).ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetTransactionDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Accounts/transactions/4/01-01-1990/12-31-3999
        [HttpGet("transactions/{id}/{startdate}/{enddate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionDetailsByAccountIDWithDateRange(int id, string startdate, string enddate)
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
                    return NotFound(null);
                }

                // Check date range.
                DateTime startDate = Convert.ToDateTime(startdate);
                DateTime endDate = Convert.ToDateTime(enddate);
                var query = result.Where(t => DateTime.Compare(startDate, t.TimeStamp) <= 0 && DateTime.Compare(endDate, t.TimeStamp) >= 0).OrderBy(t=>t.TimeStamp);
                if (query.Count() > 0)
                {
                    // Reduce result list to new query.
                    result = query.ToList();
                }
                else
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} transaction details not found in date range! {1} to {2}", id.ToString(), startDate.ToString(), endDate.ToString()));
                    return NotFound(id);
                }

                // Return list of transactions found.
                _logger?.LogInformation(string.Format("GetTransactionDetailsByAccountID: {0} Succeeded.", id.ToString()));
                return Ok(result.OrderByDescending(tr => tr.TimeStamp).ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetTransactionDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Accounts/transactions/4/2
        [HttpGet("transactions/{id}/{limit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionDetailsByAccountIDWithLimit(int id, int limit)
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
                    return NotFound(null);
                }

                // Get limit range.
                var query = result.OrderBy(t => t.TimeStamp);
                // Reduce result list to new query.
                result = query.Skip(Math.Max(0, query.Count() - limit)).ToList();
                // Check for empty limit list.
                if (result.Count() < 1)
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} transaction details limit result empty!", id.ToString()));
                    return NotFound(id);
                }

                // Return list of transactions found.
                _logger?.LogInformation(string.Format("GetTransactionDetailsByAccountID: {0} Succeeded.", id.ToString()));
                return Ok(result.OrderByDescending(tr => tr.TimeStamp).ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetTransactionDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Accounts/transactions/4/2//01-01-1990/12-31-3999
        [HttpGet("transactions/{id}/{limit}/{startdate}/{enddate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionDetailsByAccountIDWithLimitAndDateRange(int id, int limit, string startdate, string enddate)
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
                    return NotFound(null);
                }

                // Check date range.
                DateTime startDate = Convert.ToDateTime(startdate);
                DateTime endDate = Convert.ToDateTime(enddate);
                var query = result.Where(t => DateTime.Compare(startDate, t.TimeStamp) <= 0 && DateTime.Compare(endDate, t.TimeStamp) >= 0).OrderBy(t => t.TimeStamp);
                // Check query result for empty list.
                if (query.Count() > 0)
                {
                    // Reduce result list to new query.
                    result = query.Skip(Math.Max(0, query.Count() - limit)).ToList();
                    // Check for empty limit list.
                    if (result.Count() < 1)
                    {
                        // Return NotFound 404 response if no account detail was found for ID.
                        _logger?.LogWarning(string.Format("Account #{0} transaction details limit list, after date range, empty!", id.ToString()));
                        return NotFound(id);
                    }
                }
                else
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} transaction details not found in date range! {1} to {2}", id.ToString(), startDate.ToString(), endDate.ToString()));
                    return NotFound(id);
                }

                // Return list of transactions found.
                _logger?.LogInformation(string.Format("GetTransactionDetailsByAccountID: {0} Succeeded.", id.ToString()));
                return Ok(result.OrderByDescending(tr=>tr.TimeStamp).ToList());
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetTransactionDetailsByAccountID!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}