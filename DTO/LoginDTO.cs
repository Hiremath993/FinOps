using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finops.DTO
{
    public class LoginDTO
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Role{get;set;}
    }
}