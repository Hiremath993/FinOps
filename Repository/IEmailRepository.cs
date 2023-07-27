using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finops.DTO;

namespace Finops.Repository
{
    public interface IEmailRepository
    {
        void SendEmail(EmailDTO email);

        void PasswordVerificationEmail(ForgotPasswordDTO forgotPasswordDTO, string token);
    }
}