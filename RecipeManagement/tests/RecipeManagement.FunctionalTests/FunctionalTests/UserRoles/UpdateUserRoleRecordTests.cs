namespace RecipeManagement.FunctionalTests.FunctionalTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class UpdateUserRoleRecordTests : TestBase
{
    [Test]
    public async Task put_userrole_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());
        var updatedUserRoleDto = new FakeUserRoleForUpdateDto { }.Generate();

        _client.AddAuth(new[] {Roles.SuperAdmin});
        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.Put.Replace(ApiRoutes.UserRoles.Id, fakeUserRole.Id.ToString());
        var result = await _client.PutJsonRequestAsync(route, updatedUserRoleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_userrole_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());
        var updatedUserRoleDto = new FakeUserRoleForUpdateDto { }.Generate();

        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.Put.Replace(ApiRoutes.UserRoles.Id, fakeUserRole.Id.ToString());
        var result = await _client.PutJsonRequestAsync(route, updatedUserRoleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_userrole_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate(new FakeUserRoleForCreationDto().Generate());
        var updatedUserRoleDto = new FakeUserRoleForUpdateDto { }.Generate();
        _client.AddAuth();

        await InsertAsync(fakeUserRole);

        // Act
        var route = ApiRoutes.UserRoles.Put.Replace(ApiRoutes.UserRoles.Id, fakeUserRole.Id.ToString());
        var result = await _client.PutJsonRequestAsync(route, updatedUserRoleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}