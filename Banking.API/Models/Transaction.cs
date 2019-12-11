using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Models
{
    public class Transaction
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public int TransactionTypeId { get; set; }
        [Required]
        public int AccountId { get; set; }
        public int AssociatedAccountId { get; set; }
        [Required]
        public decimal Ammount { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
