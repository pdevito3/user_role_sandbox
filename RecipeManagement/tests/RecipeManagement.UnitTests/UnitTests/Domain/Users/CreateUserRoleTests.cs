namespace RecipeManagement.UnitTests.UnitTests.Domain.UserRoles;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using RecipeManagement.Domain.Roles;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.DomainEvents;

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
        // Arrange
        var userId = Guid.NewGuid();
        var role = _faker.PickRandom<RoleEnum>(RoleEnum.List).Name;
        
        // Act
        var fakeUserRole = UserRole.Create(userId, new Role(role));

        // Assert
        fakeUserRole.UserId.Should().Be(userId);
        fakeUserRole.Role.Should().Be(new Role(role));
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = _faker.PickRandom<RoleEnum>(RoleEnum.List).Name;
        
        // Act
        var fakeUserRole = UserRole.Create(userId, new Role(role));

        // Assert
        fakeUserRole.DomainEvents.Count.Should().Be(1);
        fakeUserRole.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserRolesUpdated));
    }
}