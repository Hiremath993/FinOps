using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Finops.DTO;
using Finops.Helpers;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace Finops.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration config;

        public EmailRepository(IConfiguration config)
        {
            this.config = config;
        }

        public async void SendEmail(EmailDTO request)
        {
            //Creating the message.
            var email = new MimeMessage();
            var from = config["EmailSettings:EmailUsername"];
            email.From.Add(new MailboxAddress("FinOps", from));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(FormBody.FormStringBody())
            };
            // Sending the email.
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config["EmailSettings:EmailUsername"], config["EmailSettings:EmailPassword"]);
            await Task.Delay(5000);
            smtp.Send(email);
            smtp.Disconnect(true);

        }

        public async void PasswordVerificationEmail(ForgotPasswordDTO forgotPasswordDTO, string token)
        {
            var emailMessage = new MimeMessage();
            var from = config["EmailSettings:EmailUsername"];
            emailMessage.From.Add(new MailboxAddress("SenderLink", from));
            emailMessage.To.Add(new MailboxAddress(forgotPasswordDTO.Email, forgotPasswordDTO.Email));
            emailMessage.Subject = "Reset Password Token";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(EmailBody.EmailStringBody(forgotPasswordDTO.Email, token))
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtp.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate(config["EmailSettings:EmailUsername"], config["EmailSettings:EmailPassword"]);
                    smtp.Send(emailMessage);
                    await Task.Delay(3000);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}