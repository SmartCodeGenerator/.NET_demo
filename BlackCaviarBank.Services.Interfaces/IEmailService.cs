using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
