using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required, Column(TypeName = "decimal(20,2)")]
        public decimal Ammount { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
