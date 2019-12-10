﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
    }
}
