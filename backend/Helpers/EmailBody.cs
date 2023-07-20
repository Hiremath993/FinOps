using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finops.Helpers
{
    public class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
            <head>
            </head>
            <body>
              <div>
                <h1>Reset your Password</h1>
                <hr>
                <p>You're receiving this email because you have over utilized the resources.</p>
                <p>Please click the link below to choose a new password</p>
                <a href=""http://localhost:4200/app-reset-link?email={email}&code={emailToken}"" target=""_blank"">ResetPassword</a><br>
                <p>Kind Regards,<br><br>
                FinOps Team</p>
                </div>
                </body>
            </html>";
        }
    }
}