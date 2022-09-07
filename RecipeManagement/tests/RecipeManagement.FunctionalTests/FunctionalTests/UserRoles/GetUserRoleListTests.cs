namespace RecipeManagement.FunctionalTests.FunctionalTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetUserRoleListTests : TestBase
{
    [Test]
    public async Task get_userrole_list_returns_success_using_valid_auth_credentials()
    {
        // Arrange
        

        _client.AddAuth(new[] {Roles.SuperAdmin});

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.UserRoles.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_userrole_list_returns_unauthorized_without_valid_token()
    {
        // Arrange
        // N/A

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.UserRoles.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_userrole_list_returns_forbidden_without_proper_scope()
    {
        // Arrange
        _client.AddAuth();

        // Act
        var result = await _client.GetRequestAsync(ApiRoutes.UserRoles.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}