using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finops.Helpers
{
    public class FormBody
    {
        public static string FormStringBody()
        {
            return $@"<html>
            <head>
            </head>
            <body>
              <div>
                <h4>You have Over Utilized Resources</h4>
                <hr>
                <p>You're receiving this email because you have over utilized your resources.</p>
                <p>Kind Regards,<br><br>
                FinOps Team</p>
                </div>
                </body>
            </html>";
        }
    }
}