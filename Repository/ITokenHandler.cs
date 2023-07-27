using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using resource.Models;
using Finops.Models;

namespace Finops.Repository
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(Login loginTable);
    }
}