using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.IO.File;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class PersonalCabinetService : IPersonalCabinetService
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly IRepository<UserProfile> userRepository;
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Card> cardRepository;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;

        public PersonalCabinetService(UserManager<UserProfile> userManager, IRepository<UserProfile> userRepository, IRepository<Account> accountRepository, IRepository<Card> cardRepository, IEmailService emailService, IMapper mapper)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.accountRepository = accountRepository;
            this.cardRepository = cardRepository;
            this.emailService = emailService;
            this.mapper = mapper;
        }

        public async Task BlockAccount(Guid id)
        {
            var account = await accountRepository.GetById(id);
            account.IsBlocked = true;
        }

        public async Task BlockCard(Guid id)
        {
            var card = await cardRepository.GetById(id);
            card.IsBlocked = true;
        }

        public async Task ChangePassword(UserProfile user, ChangePasswordDTO changePassword)
        {
            await userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
        }

        public async Task ChangeProfileImage(UserProfile user, string imagePath)
        {
            user.ProfileImage = await ReadAllBytesAsync(imagePath);
        }

        public void ChangeProfileInfo(UserProfile user, ProfileInfoDTO profileInfo)
        {
            mapper.Map(profileInfo, user);
        }

        public async Task ForgotPassword(ForgotPasswordDTO data, string callbackUrl)
        {
            await emailService.SendEmailAsync(data.Email, "Reset Password",
                $"To reset your password, follow the link: <a href='{callbackUrl}'>link</a>");
        }

        public async Task ResetPassword(ResetPasswordDTO data)
        {
            var user = (await userRepository.Get(u => u.Email.Equals(data.Email))).First();
            await userManager.ResetPasswordAsync(user, data.Code, data.Password);
        }

        public async Task UnblockAccount(Guid id)
        {
            var account = await accountRepository.GetById(id);
            account.IsBlocked = false;
        }

        public async Task UnblockCard(Guid id)
        {
            var card = await cardRepository.GetById(id);
            card.IsBlocked = false;
        }
    }
}
