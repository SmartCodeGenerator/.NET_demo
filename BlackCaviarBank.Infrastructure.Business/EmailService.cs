using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using BlackCaviarBank.Services.Interfaces;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Website administration", "alex.jeffrey.student@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("alex.jeffrey.student@gmail.com", "greedIsgood245");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
