using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IPersonalCabinetService
    {
        void ChangeProfileInfo(UserProfile user, ProfileInfoDTO profileInfo);
        Task ChangeProfileImage(UserProfile user, string imagePath);
        Task ChangePassword(UserProfile user, ChangePasswordDTO changePassword);
        Task ForgotPassword(ForgotPasswordDTO data, string callbackUrl);
        Task ResetPassword(ResetPasswordDTO data);
        Task BlockCard(Guid id);
        Task UnblockCard(Guid id);
        Task BlockAccount(Guid id);
        Task UnblockAccount(Guid id);
    }
}
