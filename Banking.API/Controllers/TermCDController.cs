using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermCDController : ControllerBase
    {
        int termDepositId = 4; 

        public void withdraw(Account input, decimal ammountToWithdraw)
        {
            DateTime compareDate = input.CreateDate;
            compareDate.AddYears(1);

            if (input.AccountTypeId == termDepositId && input.Balance >= ammountToWithdraw && compareDate.CompareTo(DateTime.Now) < 0)
            {
                input.Balance -= ammountToWithdraw;
            }
        }

    }
}