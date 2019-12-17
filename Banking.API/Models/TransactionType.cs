using System.ComponentModel.DataAnnotations;

namespace Banking.API.Models
{
    public class TransactionType
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
