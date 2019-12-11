using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // TODO: Replace objects with actual data types.
        // TODO: Add injection for repository when ready.

        readonly ILogger<AccountsController> _logger;
        readonly object _repo;

        public AccountsController(object newRepo, ILogger<AccountsController> newLogger)
        {
            _logger = newLogger;
            _repo = newRepo;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [Produces(typeof(IEnumerable<Account>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountsByUserID(int id)
        {
            try
            {
                IEnumerable<Account> result = null;
                _logger?.LogInformation(string.Format("Start GetAllAccountsByUserID: {0}", id.ToString()));
                // TODO: Get List of accounts from repository _repo.

                // Check if returned list has any elements.
                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response on empty list.
                    _logger?.LogWarning(string.Format("No Accounts found for UserID: {0}", id.ToString()));
                    return NotFound(id);
                }

                // Return list of accounts on successful find.
                _logger?.LogInformation(string.Format("GetAllAccountsByUserID: {0} Succeeded.", id.ToString()));
                return result.ToList();
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserID!");
                return StatusCode(500, WTF);
            }
        }

        // GET: api/Accounts/5/1
        [HttpGet("{id}/{typeid}")]
        [Produces(typeof(IEnumerable<Account>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountsByUserIDAndTypeID(int id, int typeid)
        {
            try
            {
                IEnumerable<Account> result = null;
                _logger?.LogInformation(string.Format("Start GetAllAccountsByUserID: {0}, Filtered by TypeID: {1}", id.ToString(), typeid.ToString()));
                // TODO: Get List of accounts, by typeid, from repository _repo.

                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response on empty list.
                    _logger?.LogWarning(string.Format("No Accounts found for UserID: {0}, with Filter by TypeID: {1}", id.ToString(), typeid.ToString()));
                    return NotFound(id);
                }

                // Return list of accounts on successful find.
                _logger?.LogInformation(string.Format("GetAllAccountsByUserID: {0}, Filtered by TypeID: {1} Succeeded", id.ToString(), typeid.ToString()));
                return result.ToList();
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserIDAndTypeID!");
                return StatusCode(500, WTF);
            }
        }

        // GET: api/Accounts/details/4
        [HttpGet("details/{id}")]
        [Produces(typeof(Account))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Account>> GetAccountDetailsByAccountID(int id)
        {
            try
            {
                Account result = null;
                _logger?.LogInformation(string.Format("Start GetAccountDetailsByAccountID: {0}", id.ToString()));
                // TODO: Get account detail, for id, from repository _repo.

                // Check if return object was null.
                if (result == null)
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} details not found!", id.ToString()));
                    return NotFound(id);
                }

                // Return account object found.
                _logger?.LogInformation(string.Format("GetAccountDetailsByAccountID: {0} Succeeded.", id.ToString()));

                return result;
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAccountDetailsByAccountID!");
                return StatusCode(500, WTF);
            }
        }

        // GET: api/Accounts/transactions/4
        [HttpGet("transactions/{id}")]
        [Produces(typeof(IEnumerable<Transaction>))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionDetailsByAccountID(int id)
        {
            try
            {
                IEnumerable<Transaction> result = null;
                _logger?.LogInformation(string.Format("Start GetTransactionDetailsByAccountID: {0}", id.ToString()));
                // TODO: Get transaction detail, for id, from repository _repo.

                // Check if return object was null.
                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response if no account detail was found for ID.
                    _logger?.LogWarning(string.Format("Account #{0} transaction details not found!", id.ToString()));
                    return NotFound(id);
                }

                // Return list of transactions found.
                _logger?.LogInformation(string.Format("GetTransactionDetailsByAccountID: {0} Succeeded.", id.ToString()));
                return result.ToList();
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetTransactionDetailsByAccountID!");
                return StatusCode(500, WTF);
            }
        }

        // GET: api/Accounts/transactionTypes/
        [HttpGet("transactiontypes")]
        [Produces(typeof(IEnumerable<TransactionType>))]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetAllTransactionTypes()
        {
            try
            {
                IEnumerable<TransactionType> result = null;
                _logger?.LogInformation("Start GetAllTransactionTypes.");
                // TODO: get list of transaction types from repository _repo.

                // Check for empty return set.
                if (result == null || result?.Count() < 1)
                {
                    // Return NotFound 404 response if no transaction types were found.
                    _logger?.LogWarning("No Transaction Types found!");
                    return NotFound();
                }

                // Return list of transaction types.
                _logger?.LogInformation("GetAllTransactionTypes Succeeded.");
                return result.ToList();
            }
            catch (Exception WTF)
            {
                // Return Internal Server Error 500 on general exception.
                _logger?.LogError(WTF, "Unexpected Error in GetAllTransactionTypes!");
                return StatusCode(500, WTF);
            }
        }
    }
}