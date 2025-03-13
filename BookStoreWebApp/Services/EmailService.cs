﻿using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BookStoreWebApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
            var enableSSL = bool.Parse(_config["EmailSettings:EnableSSL"] ?? "true");

            var smtpUser = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? _config["EmailSettings:SmtpUsername"];
            var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? _config["EmailSettings:SmtpPassword"];
            var senderEmail = _config["EmailSettings:SenderEmail"] ?? smtpUser;
            var senderName = _config["EmailSettings:SenderName"] ?? "Book Store Admin";

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(senderName, senderEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls); // STARTTLS
            await smtp.AuthenticateAsync(smtpUser, smtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
