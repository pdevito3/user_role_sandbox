namespace RecipeManagement.FunctionalTests.FunctionalTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetUserRoleTests : TestBase
{
    [Test]
    public async Task get_userrole_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());

        _client.AddAuth(new[] {Roles.SuperAdmin});
        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.GetRecord.Replace(ApiRoutes.UserRoles.Id, fakeUserRole.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_userrole_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());

        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.GetRecord.Replace(ApiRoutes.UserRoles.Id, fakeUserRole.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_userrole_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.GetRecord.Replace(ApiRoutes.UserRoles.Id, fakeUserRole.Id.ToString());
        var result = await _client.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}