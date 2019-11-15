using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "login@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("username", "password");
            }
        }
    }
}
