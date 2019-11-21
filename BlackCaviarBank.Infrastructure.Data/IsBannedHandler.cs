using BlackCaviarBank.Infrastructure.Data.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class IsBannedHandler : AuthorizationHandler<IsBannedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsBannedRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "isBanned"))
            {
                if (context.User.FindFirst(c => c.Type == "isBanned").Equals(requirement.IsBanned))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
