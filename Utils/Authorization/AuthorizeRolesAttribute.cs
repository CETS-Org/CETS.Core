using Microsoft.AspNetCore.Authorization;

namespace Utils.Authorization
{
    /// <summary>
    /// Custom authorization attribute for role-based access control
    /// Usage: [AuthorizeRoles("Admin", "Teacher")]
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}

