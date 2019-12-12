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
    public class AccountTypesApiController : ControllerBase
    {
        private readonly IAccountTypeRepo _context;
        //private readonly ILogger<AccountTypesApiController> _logger;

        public AccountTypesApiController(IAccountTypeRepo ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountType>>> GetAccountTypes()
        {
            return await _context.GetAccountTypes();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountType>> GetAccountTypeById(int id)
        {
            AccountType accType = await _context.GetAccountTypeById(id);
            if(accType is null) { return NotFound(); }
            else { return accType; }
        }

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<AccountType>> GetAccountTypeByName(string name)
        {
            AccountType accType = await _context.GetAccountTypeByName(name);
            if (accType is null) { return NotFound(); }
            else { return accType; }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAccountType(AccountType accType)
        {
            await _context.AddAccountType(accType);
            return true;
        }
    }
}