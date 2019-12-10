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
        [HttpGet]
        [Produces(typeof(object))]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
    }
}