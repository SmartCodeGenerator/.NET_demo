using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<IdentityError>> Register(UserManager<UserProfile> userManager, IMapper mapper, RegisterUserDTO userDTO);
    }
}
