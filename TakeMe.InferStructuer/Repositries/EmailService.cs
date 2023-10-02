using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.DTOs;
using TakeMe.Core.Interfaces;

namespace TakeMe.InferStructuer.Repositries
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void sendEmail(EmailModelDTO email)
        {
            MimeMessage Email = new MimeMessage();
            string from = configuration["EmailSettings:From"];
            Email.From.Add(new MailboxAddress("ahmad222jarad", from));
            Email.To.Add(new MailboxAddress(email.To, email.To));
            Email.Subject = email.Subject;
            Email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content),
            };
            using (var Client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    Client.Connect(configuration["EmailSettings:SmtpServer"], 465, true);
                    Client.Authenticate(configuration["EmailSettings:From"], configuration["EmailSettings:Password"]);
                    Client.Send(Email);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    Client.Disconnect(true);
                    Client.Dispose();
                }
            }
        }
    }
}
