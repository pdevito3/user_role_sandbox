using RecipeManagement.Domain.UserRoles.Services;

namespace RecipeManagement.Services;

using System.Security.Claims;
using RecipeManagement.Domain.RolePermissions.Services;
using SharedKernel.Domain;
using RecipeManagement.Domain;
using HeimGuard;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

public class UserPolicyHandler : IUserPolicyHandler
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRoleRepository _userRoleRepository;

    public UserPolicyHandler(IRolePermissionRepository rolePermissionRepository, ICurrentUserService currentUserService, IUserRoleRepository userRoleRepository)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _currentUserService = currentUserService;
        _userRoleRepository = userRoleRepository;
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
            return _userRoleRepository.GetRolesByUserSid(userSid).ToArray();
        
        // TODO -- add a clientId column and change entity to `RoleMappings`???
        if(!string.IsNullOrEmpty(clientId))
            return _userRoleRepository.GetRolesByUserSid(userSid).ToArray();

        return Array.Empty<string>();
    }

    private class RealmAccess
    {
        public string[] Roles { get; set; }
    }
}