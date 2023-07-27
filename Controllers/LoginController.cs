using Finops.DTO;
using Finops.Repository.ILogin;
using Finops.Repository;
using Microsoft.AspNetCore.Mvc;
using resource.Models;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ctrlspec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _Login;
        private readonly ITokenHandler handler;


        public LoginController(ILogin _Login, ITokenHandler handler)
        {

            this._Login = _Login;
            this.handler = handler;
        }
        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult<LoginRepository>> SignUp(Login loginTable)

        {
            var add = await _Login.SignUp(loginTable);

            if (loginTable == null)
            {
                return Ok("Bad request");
            }

            return Ok("Success");
        }

       [HttpPost]
        [Route("LoginAsync")]
        public async Task<IActionResult> LoginAsync(LoginDTO LoginDTO)
        {
            try
            {
                if 
                (LoginDTO.EmailId == null && LoginDTO.Password == null && LoginDTO.Role == null)
                {
                    return NotFound("EmailId or password is null");
                
                }
                //we check if user is authenticated which is check the username and password is present 
                // in our database.
                var user = await _Login.Login(LoginDTO.EmailId, LoginDTO.Password,LoginDTO.Role);
                if (user != null)
                {
                    var token = handler.CreateTokenAsync(user);
                    return Ok(token);
                }
                
                return BadRequest("Emailid or password or Role is incorrect ");
          

            }
        
                    //generate jwt token
            
        
             catch (Exception e)
             {
                 return BadRequest("Error in Controller method LoginAsync"+ e);
             }
        
    }


        [HttpGet("EmailService")]
        public IActionResult SendEmail(string name, string receiver)
        {
            string body = "Thanks " + name + "!\n\n Your email id " + receiver + " is succesfully registered with" +
            "CarWashExpress \n\n Regards,\n CarWashExpress Ltd.\n Contact us: carwash240130@gmail.com";
            var email = new MimeMessage(); email.From.Add(MailboxAddress.Parse("carwash240130@gmail.com"));
            email.To.Add(MailboxAddress.Parse(receiver));
            email.Subject = "Registration comfirmation mail-CarWashExpress";
            email.Body = new TextPart(TextFormat.Plain) { Text = body };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("finops902@gmail.com", "rnztmcqgxdsxwvkp"); //email and password
            smtp.Send(email);
            smtp.Disconnect(true); return Ok("Mail Sended ");
        }

    }

}


