namespace RecipeManagement.UnitTests.UnitTests.Domain.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Domain.UserRoles.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class CreateUserRoleTests
{
    private readonly Faker _faker;

    public CreateUserRoleTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_userRole()
    {
        // Arrange + Act
        var fakeUserRole = FakeUserRole.Generate();

        // Assert
        fakeUserRole.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeUserRole = FakeUserRole.Generate();

        // Assert
        fakeUserRole.DomainEvents.Count.Should().Be(1);
        fakeUserRole.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserRoleCreated));
    }
}