using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using resource.Models;
using Finops.Models;

namespace Finops.Repository.ILogin
{
    public interface ILogin
    {
        IEnumerable<object> ILogin { get; }
        IEnumerable<object> LoginAsync { get; }

        Task<Login> SignUp(Login loginTable);
       Task<Login> Login(string emailId, string Password, string Role);
    }
}