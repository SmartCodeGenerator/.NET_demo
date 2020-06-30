using BlackCaviarBank.Services.Interfaces;
using MimeKit;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            _ = new MailAddress(email);

            if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(message))
            {
                subject = "BCB notification";
                message = "This message is sent for unmanaged purpose";
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Black Caviar Bank administration", "alexjfr112@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("alexjfr112@gmail.com", "fakerfaker24");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
