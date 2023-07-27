using Finops.Repository;
using Finops.Repository.ILogin;

namespace Finops
{
    internal class loginService
    {
        private ILogin _loginRepo;

        public loginService(ILogin loginRepo)
        {
            _loginRepo = loginRepo;
        }
    }
}