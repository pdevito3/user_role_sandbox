namespace RecipeManagement.IntegrationTests.FeatureTests.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.Domain.UserRoles.Dtos;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.UserRoles.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.User;

public class UpdateUserRoleCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_userrole_in_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeUserRoleOne = FakeUserRole.Generate(new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate());
        var updatedUserRoleDto = new FakeUserRoleForUpdateDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate();
        await InsertAsync(fakeUserRoleOne);

        var userRole = await ExecuteDbContextAsync(db => db.UserRoles
            .FirstOrDefaultAsync(u => u.Id == fakeUserRoleOne.Id));
        var id = userRole.Id;

        // Act
        var command = new UpdateUserRole.Command(id, updatedUserRoleDto);
        await SendAsync(command);
        var updatedUserRole = await ExecuteDbContextAsync(db => db.UserRoles.FirstOrDefaultAsync(u => u.Id == id));

        // Assert
        updatedUserRole.Should().BeEquivalentTo(updatedUserRoleDto, options =>
            options.ExcludingMissingMembers());
    }
}