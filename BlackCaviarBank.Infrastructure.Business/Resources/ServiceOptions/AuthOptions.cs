using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlackCaviarBank.Infrastructure.Business.Resources.ServiceOptions
{
    public class AuthOptions
    {
        public const string ISSUER = "BCB_Server";
        public const string AUDIENCE = "BCB_Client";
        const string KEY = "black_caviar_bank_secret_key!1";
        public const int LIFETIME = 720;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
