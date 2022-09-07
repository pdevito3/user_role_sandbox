namespace RecipeManagement.Services;

using Domain.Users.Services;
using RecipeManagement.Domain.RolePermissions.Services;
using SharedKernel.Domain;
using RecipeManagement.Domain;
using HeimGuard;
using Microsoft.EntityFrameworkCore;

public class UserPolicyHandler : IUserPolicyHandler
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public UserPolicyHandler(IRolePermissionRepository rolePermissionRepository, ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<string>> GetUserPermissions()
    {
        var claimsPrincipal = _currentUserService.User;
        if (claimsPrincipal == null) throw new ArgumentNullException(nameof(claimsPrincipal));
        
        var userId = _currentUserService.UserId;

        // todo does the current user service implementation also grab client_id?
        var clientId = claimsPrincipal
            .Claims
            .Where(c => c.Type is "client_id")
            .Select(r => r.Value)
            .FirstOrDefault();
        var roles = GetRoles(userId, clientId);

        if (roles.Length == 0)
            throw new Exception("This user has no roles assigned. Please contact an admin to be assigned a role.");
            // TODO custom exception and handler? 500 or custom 4xx?

        // super admins can do everything
        if(roles.Contains(Roles.SuperAdmin))
            return Permissions.List();

        var permissions = await _rolePermissionRepository.Query()
            .Where(rp => roles.Contains(rp.Role))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToArrayAsync();

        return await Task.FromResult(permissions);
    }

    private string[] GetRoles(string userSid, string clientId)
    {
        if(!string.IsNullOrEmpty(userSid))
            return _userRepository.GetRolesByUserSid(userSid).ToArray();
        
        // TODO -- add a clientId column and change entity to `RoleMappings`???
        if(!string.IsNullOrEmpty(clientId))
            return _userRepository.GetRolesByUserSid(userSid).ToArray();

        return Array.Empty<string>();
    }

    private class RealmAccess
    {
        public string[] Roles { get; set; }
    }
}