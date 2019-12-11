using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int TransactionTypeId { get; set; }
        public int AccountId { get; set; }
        public int AssociatedAccountId { get; set; }
        public decimal Ammount { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
