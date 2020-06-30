using BlackCaviarBank.Infrastructure.Business;
using System;
using Xunit;

namespace BlackCaviarBank.Tests
{
    public class EmailServiceTests
    {
        [Fact]
        public async void SendEmailAsync_CorrectEmailFormat()
        {
            var service = new EmailService();

            await service.SendEmailAsync("alexmyt112@gmail.com", "", null);
        }

        [Fact]
        public async void SendEmailAsync_WrongEmailFormat()
        {
            var service = new EmailService();

            await Assert.ThrowsAsync(new FormatException().GetType(), () => service.SendEmailAsync("dqwedqwed", "test email", "test"));
        }
    }
}
