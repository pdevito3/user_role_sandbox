namespace RecipeManagement.IntegrationTests.FeatureTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.Domain.UserRoles.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.User;

public class UserRoleQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_userrole_with_accurate_props()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeUserRoleOne = FakeUserRole.Generate(new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate());
        await InsertAsync(fakeUserRoleOne);

        // Act
        var query = new GetUserRole.Query(fakeUserRoleOne.Id);
        var userRole = await SendAsync(query);

        // Assert
        userRole.Should().BeEquivalentTo(fakeUserRoleOne, options =>
            options.ExcludingMissingMembers());
    }

    [Test]
    public async Task get_userrole_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetUserRole.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}