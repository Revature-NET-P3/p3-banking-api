﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.API.Models
{
    public class AccountType
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Column(TypeName = "decimal(20,2)")]
        public decimal InterestRate { get; set; }
    }
}
