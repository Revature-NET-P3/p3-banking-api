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

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermCDController : ControllerBase
    {
        int termDepositId = 4;
        private readonly IAccountRepo _Context;

        public TermCDController(IAccountRepo ctx)
        {
            _Context = ctx;
        }

        public void Withdraw(Account input, decimal ammountToWithdraw)
        {
            DateTime compareDate = input.CreateDate;
            compareDate.AddYears(1);

            if (input.AccountTypeId == termDepositId && input.Balance >= ammountToWithdraw && compareDate.CompareTo(DateTime.Now) < 0)
            {
                input.Balance -= ammountToWithdraw;
            }
        }

        public async Task<IActionResult> AddTermCD(Account addMe)
        {
            if (addMe.AccountTypeId == termDepositId)
            {
                _Context.OpenAccount(addMe);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}