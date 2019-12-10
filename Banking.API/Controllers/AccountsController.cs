using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }

        // GET: api/Accounts
        [HttpGet("{id}")]
        [Produces(typeof(object))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAccountsByUserID(int id)
        {
            try
            {
                IEnumerable<object> result = null;

                _logger.LogInformation(string.Format("Start GetAllAccountsByUserID: {0}", id.ToString()));

                if (result.Count() < 1)
                {
                    _logger.LogWarning(string.Format("No Accounts found for UserID: {0}", id.ToString()));
                    return NotFound(id);
                }
                _logger.LogInformation(string.Format("GetAllAccountsByUserID: {0} Succeded.", id.ToString()));

                return result;
            }
            catch (Exception WTF)
            {
                _logger?.LogError(WTF, "Unexpected Error in GetAllAccountsByUserID!");
                return StatusCode(500, WTF);
            }
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
    }
}