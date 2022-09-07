namespace RecipeManagement.UnitTests.UnitTests.Domain.Users;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateUserTests
{
    private readonly Faker _faker;

    public CreateUserTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_user()
    {
        // Arrange + Act
        var fakeUser = FakeUser.Generate();

        // Assert
        fakeUser.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeUser = FakeUser.Generate();

        // Assert
        fakeUser.DomainEvents.Count.Should().Be(1);
        fakeUser.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserCreated));
    }
}