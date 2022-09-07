namespace RecipeManagement.IntegrationTests.FeatureTests.Users;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class UserQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_user_with_accurate_props()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        // Act
        var query = new GetUser.Query(fakeUserOne.Id);
        var user = await SendAsync(query);

        // Assert
        user.Should().BeEquivalentTo(fakeUserOne, options =>
            options.ExcludingMissingMembers());
    }

    [Test]
    public async Task get_user_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetUser.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}