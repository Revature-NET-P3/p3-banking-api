using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Banking.API.Controllers
{
    [EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypesApiController : ControllerBase
    {
        private readonly IAccountTypeRepo _context;
        private readonly ILogger _logger;

        public AccountTypesApiController(IAccountTypeRepo ctx, ILogger<AccountTypesApiController> logger)
        {
            _context = ctx;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountType>>> GetAccountTypes()
        {
            var accountTypes =  await _context.GetAccountTypes();
            if (accountTypes is null) { _logger.LogError("GetAccountTypes returned NULL"); }
            else {_logger.LogInformation("GetAccountTypes returned AccountTypes");}
            return accountTypes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountType>> GetAccountTypeById(int id)
        {
            AccountType accType = await _context.GetAccountTypeById(id);
            if(accType is null) {
                _logger.LogError("Account type not found with Id: {0}", id);
                return NotFound(); 
            }
            else {
                _logger.LogInformation("Account type with Id: {0} Successfully returned.", id);
                return accType; 
            }
        }

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<AccountType>> GetAccountTypeByName(string name)
        {
            AccountType accType = await _context.GetAccountTypeByName(name);
            if (accType is null) {
                _logger.LogError("Account type not found with Name: {0}", name);
                return NotFound(); 
            }
            else {
                _logger.LogInformation("Account type with Name: {0} Successfully returned.", name);
                return accType; 
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAccountType(AccountType accType)
        {
            await _context.AddAccountType(accType);
            _logger.LogInformation("Attempt to add new Account Type preformed.");
            return true;
        }
    }
}