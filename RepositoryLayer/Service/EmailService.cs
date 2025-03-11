using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Net;
using System.Net.Mail;

namespace RepositoryLayer.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_config["SMTP:Host"], int.Parse(_config["SMTP:Port"]))
                {
                    Credentials = new NetworkCredential(_config["SMTP:Username"], _config["SMTP:Password"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["SMTP:Username"]), 
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(to));

                client.Send(mailMessage); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}", ex);
            }
        }
    }
}
