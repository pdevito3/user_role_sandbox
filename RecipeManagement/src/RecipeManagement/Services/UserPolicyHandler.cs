namespace RecipeManagement.Services;

using Domain.Roles;
using Domain.Users.Dtos;
using Domain.Users.Features;
using Domain.Users.Services;
using RecipeManagement.Domain.RolePermissions.Services;
using SharedKernel.Domain;
using RecipeManagement.Domain;
using HeimGuard;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UserPolicyHandler : IUserPolicyHandler
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;

    public UserPolicyHandler(IRolePermissionRepository rolePermissionRepository, ICurrentUserService currentUserService, IUserRepository userRepository, IMediator mediator)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _mediator = mediator;
    }
    
    public async Task<IEnumerable<string>> GetUserPermissions()
    {
        var claimsPrincipal = _currentUserService.User;
        if (claimsPrincipal == null) throw new ArgumentNullException(nameof(claimsPrincipal));
        
        var userId = _currentUserService.UserId;
        var usersExist = _userRepository.Query().Any();
        if (!usersExist)
            await SeedRootUser(userId);

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

    private async Task SeedRootUser(string userId)
    {
        // TODO if machine, return error and tell them to login with a user?
        var rootUser = new UserForCreationDto()
        {
            Username = _currentUserService.Username,
            Email = _currentUserService.Email,
            FirstName = _currentUserService.FirstName,
            LastName = _currentUserService.LastName,
            Sid = userId
        };
        var userCommand = new AddUser.Command(rootUser, true);
        var createdUser = await _mediator.Send(userCommand);

        var roleCommand = new AddUserRole.Command(createdUser.Id, Role.SuperAdmin().Value, true);
        await _mediator.Send(roleCommand);
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
}