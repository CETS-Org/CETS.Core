using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Utils.Authorization
{
    /// <summary>
    /// Handler for role-based authorization requirements
    /// </summary>
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            // Get roles from claims
            var userRoles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            // Check if user has any of the required roles
            if (requirement.Roles.Any(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

