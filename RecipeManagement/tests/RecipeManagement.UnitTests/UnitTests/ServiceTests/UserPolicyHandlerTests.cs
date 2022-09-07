using Moq.Language.Flow;

namespace RecipeManagement.UnitTests.UnitTests.ServiceTests;

using RecipeManagement.Services;
using RecipeManagement.Domain.RolePermissions.Services;
using RecipeManagement.Domain;
using RecipeManagement.Domain.RolePermissions;
using RecipeManagement.Domain.RolePermissions.Dtos;
using SharedKernel.Domain;
using Bogus;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Security.Claims;
using MediatR;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Services;
using SharedTestHelpers.Fakes.User;

[Parallelizable]
public class UserPolicyHandlerTests
{
    private readonly Faker _faker;

    public UserPolicyHandlerTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void GetUserPermissions_should_require_user()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        // Act
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService
            .Setup(c => c.User)
            .Returns(claimsPrincipal);
        var rolePermissionsRepo = new Mock<IRolePermissionRepository>();
        var userRepo = new Mock<IUserRepository>();
        var mediator = new Mock<IMediator>();

        var userPolicyHandler = new UserPolicyHandler(rolePermissionsRepo.Object, currentUserService.Object, userRepo.Object, mediator.Object);
        
        Func<Task> permissions = () => userPolicyHandler.GetUserPermissions();
        
        // Assert
        permissions.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Test]
    public async Task superadmin_user_gets_all_permissions()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var userRepo = new Mock<IUserRepository>();
        userRepo.UsersExist();
        userRepo.SetUserRole(Roles.SuperAdmin);
        
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.SetCurrentUser();
        var rolePermissionsRepo = new Mock<IRolePermissionRepository>();

        // Act
        var userPolicyHandler = new UserPolicyHandler(rolePermissionsRepo.Object, currentUserService.Object, userRepo.Object, mediator.Object);
        var permissions = await userPolicyHandler.GetUserPermissions();
        
        // Assert
        permissions.Should().BeEquivalentTo(Permissions.List().ToArray());
    }
    
    // [Test]
    // public async Task superadmin_machine_gets_all_permissions()
    // {
    //     // Arrange
    //     var mediator = new Mock<IMediator>();
    //     var userRepo = new Mock<IUserRepository>();
    //     userRepo.UsersExist();
    //     var currentUserService = new Mock<ICurrentUserService>();
    //     currentUserService
    //         .Setup(c => c.User)
    //         .Returns(SetMachineClaim());
    //     var rolePermissionsRepo = new Mock<IRolePermissionRepository>();
    //     
    //     userRepo.SetUserRole(Roles.SuperAdmin);
    //
    //     // Act
    //     var userPolicyHandler = new UserPolicyHandler(rolePermissionsRepo.Object, currentUserService.Object, userRepo.Object, mediator.Object);
    //     var permissions = await userPolicyHandler.GetUserPermissions();
    //     
    //     // Assert
    //     permissions.Should().BeEquivalentTo(Permissions.List().ToArray());
    // }
    
    // [Test]
    // public async Task non_super_admin_gets_assigned_permissions_only()
    // {
    //     // Arrange
    //     var permissionToAssign = _faker.PickRandom(Permissions.List());
    //     var randomOtherPermission = _faker.PickRandom(Permissions.List().Where(p => p != permissionToAssign));
    //     var nonSuperAdminRole = _faker.PickRandom(Roles.List().Where(p => p != Roles.SuperAdmin));
    //     var user = SetUserRole(nonSuperAdminRole);
    //
    //     var rolePermission = RolePermission.Create(new RolePermissionForCreationDto()
    //     {
    //         Role = nonSuperAdminRole,
    //         Permission = permissionToAssign
    //     });
    //     var rolePermissions = new List<RolePermission>() {rolePermission};
    //     var mockData = rolePermissions.AsQueryable().BuildMock();
    //     
    //     // Act
    //     var currentUserService = new Mock<ICurrentUserService>();
    //     currentUserService
    //         .Setup(c => c.User)
    //         .Returns(user);
    //     var rolePermissionsRepo = new Mock<IRolePermissionRepository>();
    //     var userRepo = new Mock<IUserRepository>();
    //     rolePermissionsRepo
    //         .Setup(c => c.Query())
    //         .Returns(mockData);
    //
    //     var userPolicyHandler = new UserPolicyHandler(rolePermissionsRepo.Object, currentUserService.Object, userRepo.Object, mediator.Object);
    //     var permissions = await userPolicyHandler.GetUserPermissions();
    //     
    //     // Assert
    //     permissions.Should().Contain(permissionToAssign);
    //     permissions.Should().NotContain(randomOtherPermission);
    // }
    //
    // [Test]
    // public async Task claims_role_duplicate_permissions_removed()
    // {
    //     // Arrange
    //     var permissionToAssign = _faker.PickRandom(Permissions.List());
    //     var nonSuperAdminRole = _faker.PickRandom(Roles.List().Where(p => p != Roles.SuperAdmin));
    //     var user = SetUserRole(nonSuperAdminRole);
    //
    //     var rolePermission = RolePermission.Create(new RolePermissionForCreationDto()
    //     {
    //         Role = nonSuperAdminRole,
    //         Permission = permissionToAssign
    //     });
    //     var rolePermissions = new List<RolePermission>() {rolePermission, rolePermission};
    //     var mockData = rolePermissions.AsQueryable().BuildMock();
    //     
    //     // Act
    //     var currentUserService = new Mock<ICurrentUserService>();
    //     currentUserService
    //         .Setup(c => c.User)
    //         .Returns(user);
    //     var rolePermissionsRepo = new Mock<IRolePermissionRepository>();
    //     var userRepo = new Mock<IUserRepository>();
    //     rolePermissionsRepo
    //         .Setup(c => c.Query())
    //         .Returns(mockData);
    //
    //     var userPolicyHandler = new UserPolicyHandler(rolePermissionsRepo.Object, currentUserService.Object, userRepo.Object, mediator.Object);
    //     var permissions = await userPolicyHandler.GetUserPermissions();
    //     
    //     // Assert
    //     permissions.Count(p => p == permissionToAssign).Should().Be(1);
    //     permissions.Should().Contain(permissionToAssign);
    // }
    
}

public static class UserExtensions
{
    public static void SetUserRole(this Mock<IUserRepository> repo, string role)
    {
        repo
            .Setup(x => x.GetRolesByUserSid(It.IsAny<string>()))
            .Returns(new List<string> { role });
    }
    

    public static void UsersExist(this Mock<IUserRepository> repo)
    {
        var user = FakeUser.Generate();
        var users = new List<User>() {user};
        var mockData = users.AsQueryable().BuildMock();
        
        repo
            .Setup(c => c.Query())
            .Returns(mockData);
    }
}
public static class CurrentUserServiceExtensions
{
    public static void SetCurrentUser(this Mock<ICurrentUserService> repo)
    {
        var user = SetUserClaim();
        repo
            .Setup(c => c.User)
            .Returns(user);
        repo
            .Setup(c => c.UserId)
            .Returns(user?.FindFirstValue(ClaimTypes.NameIdentifier));
    }
    
    private static ClaimsPrincipal SetUserClaim(string sid = null)
    {
        sid ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, sid)
        };

        var identity = new ClaimsIdentity(claims);
        return new ClaimsPrincipal(identity);
    }
    
    private static ClaimsPrincipal SetMachineClaim(string clientId = null)
    {
        clientId ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim("client_id", clientId)
        };

        var identity = new ClaimsIdentity(claims);
        return new ClaimsPrincipal(identity);
    }
}