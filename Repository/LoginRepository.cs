using Finops.Data;
using resource.Models;
using Finops.Models;
using Microsoft.EntityFrameworkCore;

namespace Finops.Repository.ILogin
{
    public class LoginRepository : ILogin
    {
        private readonly FinopsDbContext _context;

        IEnumerable<object> ILogin.ILogin => throw new NotImplementedException();

        IEnumerable<object> ILogin.LoginAsync => throw new NotImplementedException();

        public LoginRepository(FinopsDbContext context)

        {

            _context = context;

        }


         #region AddLoginDetailsAsync
        public async Task<Login> SignUp (Login loginTable)
        {
            try
            {
                await _context.Login.AddAsync(loginTable);
                await _context.SaveChangesAsync();
                return loginTable;
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

        #region AuthenticateUserAsync
        public async Task<Login> Login (string emailId,string Password,string Role)
        {
          
                var user = await _context.Login.FirstOrDefaultAsync(x => x.EmailId == emailId && x.Password == Password && x.Role == Role);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            // catch(Exception)
            // {
            //     throw;
         // }
     
        #endregion
    }

}

