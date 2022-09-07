namespace RecipeManagement.IntegrationTests.FeatureTests.UserRoles;

using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.UserRoles.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.User;

public class UserRoleListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_userrole_list()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
    var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto().Generate());
    await InsertAsync(fakeUserOne, fakeUserTwo);

        var fakeUserRoleOne = FakeUserRole.Generate(new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate());
        var fakeUserRoleTwo = FakeUserRole.Generate(new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserTwo.Id)
            .Generate());
        var queryParameters = new UserRoleParametersDto();

        await InsertAsync(fakeUserRoleOne, fakeUserRoleTwo);

        // Act
        var query = new GetUserRoleList.Query(queryParameters);
        var userRoles = await SendAsync(query);

        // Assert
        userRoles.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}