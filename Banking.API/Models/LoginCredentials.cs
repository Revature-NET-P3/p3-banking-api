using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Models
{
    public class LoginCredentials
    {
        public string Username { get; set; }
        public string Passhash { get; set; }
    }
}
