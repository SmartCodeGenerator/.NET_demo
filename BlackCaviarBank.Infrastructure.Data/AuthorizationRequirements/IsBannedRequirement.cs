using Microsoft.AspNetCore.Authorization;

namespace BlackCaviarBank.Infrastructure.Data.AuthorizationRequirements
{
    public class IsBannedRequirement : IAuthorizationRequirement
    {
        protected internal bool IsBanned { get; set; }

        public IsBannedRequirement(bool isBanned)
        {
            IsBanned = isBanned;
        }
    }
}
