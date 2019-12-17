using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.API.Models
{
    public class Account
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int AccountTypeId { get; set; }
        public string AccNickname { get; set; }
        [Required, Column(TypeName = "decimal(20,2)")]
        public decimal Balance { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public bool IsClosed { get; set; }
    }
}
