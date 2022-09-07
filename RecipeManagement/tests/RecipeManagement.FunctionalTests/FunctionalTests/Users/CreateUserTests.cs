namespace RecipeManagement.FunctionalTests.FunctionalTests.Users;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.FunctionalTests.TestUtilities;
using RecipeManagement.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateUserTests : TestBase
{
    [Test]
    public async Task create_user_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeUser = new FakeUserForCreationDto { }.Generate();

        _client.AddAuth(new[] {Roles.SuperAdmin});

        // Act
        var route = ApiRoutes.Users.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUser);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_user_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());

        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUser);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_user_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeUser = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        _client.AddAuth();

        await InsertAsync(fakeUser);

        // Act
        var route = ApiRoutes.Users.Create;
        var result = await _client.PostJsonRequestAsync(route, fakeUser);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}