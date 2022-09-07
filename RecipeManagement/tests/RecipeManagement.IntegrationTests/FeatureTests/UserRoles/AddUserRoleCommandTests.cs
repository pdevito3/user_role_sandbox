using RecipeManagement.Domain.Roles;

namespace RecipeManagement.IntegrationTests.FeatureTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using RecipeManagement.Domain.UserRoles.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using RecipeManagement.SharedTestHelpers.Fakes.User;

public class AddUserRoleCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_userrole_to_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeUserRoleOne = new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate();

        // Act
        var command = new AddUserRole.Command(fakeUserRoleOne);
        var userRoleReturned = await SendAsync(command);
        var userRoleCreated = await ExecuteDbContextAsync(db => db.UserRoles
            .FirstOrDefaultAsync(u => u.Id == userRoleReturned.Id));

        // Assert
        userRoleReturned.Should().BeEquivalentTo(fakeUserRoleOne, options =>
            options.ExcludingMissingMembers());
        userRoleCreated.Should().BeEquivalentTo(fakeUserRoleOne, options =>
            options.ExcludingMissingMembers()
                .Excluding(x => x.Role));
        userRoleCreated.Role.Should().BeEquivalentTo(new Role(fakeUserRoleOne.Role));
    }
}