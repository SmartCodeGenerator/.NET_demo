using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.IO.File;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class RegistrationService : IRegistrationService
    {
        public async Task<IEnumerable<IdentityError>> Register(UserManager<UserProfile> userManager, IMapper mapper, RegisterUserDTO userDTO)
        {
            UserProfile user = mapper.Map<UserProfile>(userDTO);
            user.ProfileImage = !string.IsNullOrEmpty(userDTO.ProfileImageUrl) ? 
                await ReadAllBytesAsync(userDTO.ProfileImageUrl) : 
                await ReadAllBytesAsync("./MyStaticFiles/images/user_icon.svg");

            IdentityResult result = await userManager.CreateAsync(user, userDTO.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "user");

                return null;
            }
            else
            {
                return result.Errors;
            }
        }
    }
}
