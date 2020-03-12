using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Business.Resources.ServiceOptions;
using BlackCaviarBank.Services.Interfaces;
using BlackCaviarBank.Services.Interfaces.Resources.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<UserProfile> userManager;

        public AuthenticationService(UserManager<UserProfile> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> Authenticate(LoginUserDTO userDTO)
        {
            UserProfile user = await userManager.FindByNameAsync(userDTO.UserName);

            PasswordVerificationResult result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userDTO.Password);

            if (result == PasswordVerificationResult.Success)
            {
                IList<string> roles = await userManager.GetRolesAsync(user);

                var claims = new List<Claim>();

                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
                }
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));

                var now = DateTime.UtcNow;

                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            return "";
        }
    }
}
