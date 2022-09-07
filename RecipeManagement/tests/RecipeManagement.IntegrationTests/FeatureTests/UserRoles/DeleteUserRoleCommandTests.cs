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

public class DeleteUserRoleCommandTests : TestBase
{
    [Test]
    public async Task can_delete_userrole_from_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeUserRoleOne = FakeUserRole.Generate(new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate());
        await InsertAsync(fakeUserRoleOne);
        var userRole = await ExecuteDbContextAsync(db => db.UserRoles
            .FirstOrDefaultAsync(u => u.Id == fakeUserRoleOne.Id));

        // Act
        var command = new DeleteUserRole.Command(userRole.Id);
        await SendAsync(command);
        var userRoleResponse = await ExecuteDbContextAsync(db => db.UserRoles.CountAsync(u => u.Id == userRole.Id));

        // Assert
        userRoleResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_userrole_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteUserRole.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_userrole_from_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeUserRoleOne = FakeUserRole.Generate(new FakeUserRoleForCreationDto()
            .RuleFor(u => u.UserId, _ => fakeUserOne.Id)
            .Generate());
        await InsertAsync(fakeUserRoleOne);
        var userRole = await ExecuteDbContextAsync(db => db.UserRoles
            .FirstOrDefaultAsync(u => u.Id == fakeUserRoleOne.Id));

        // Act
        var command = new DeleteUserRole.Command(userRole.Id);
        await SendAsync(command);
        var deletedUserRole = await ExecuteDbContextAsync(db => db.UserRoles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == userRole.Id));

        // Assert
        deletedUserRole?.IsDeleted.Should().BeTrue();
    }
}