namespace RecipeManagement.FunctionalTests.FunctionalTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateUserRoleTests : TestBase
{
    [Test]
    public async Task create_userrole_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeUserRole = new FakeUserRoleForCreationDto { }.Generate();

        _client.AddAuth(new[] {Roles.SuperAdmin});

        // Act
        var route = ApiRoutes.UserRoles.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUserRole);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_userrole_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());

        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUserRole);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_userrole_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUserRole);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}