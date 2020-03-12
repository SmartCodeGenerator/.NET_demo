using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using System.Threading.Tasks;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(LoginUserDTO userDTO);
    }
}
