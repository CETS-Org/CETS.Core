using Microsoft.AspNetCore.Authorization;

namespace Utils.Authorization
{
    /// <summary>
    /// Custom authorization requirement for role-based access
    /// </summary>
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; }

        public RoleRequirement(params string[] roles)
        {
            Roles = roles;
        }
    }
}

