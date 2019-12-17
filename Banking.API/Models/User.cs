using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.API.Models
{
    public class User
    {
        [Required, Key]
        public int Id { get; set; }
        [Required, Index(IsUnique = true)]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
