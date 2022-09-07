namespace RecipeManagement.UnitTests.UnitTests.Domain.UserRoles;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Domain.UserRoles.DomainEvents;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

[Parallelizable]
public class UpdateUserRoleTests
{
    private readonly Faker _faker;

    public UpdateUserRoleTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_userRole()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate();
        var updatedUserRole = new FakeUserRoleForUpdateDto().Generate();
        
        // Act
        fakeUserRole.Update(updatedUserRole);

        // Assert
        fakeUserRole.Should().BeEquivalentTo(updatedUserRole, options =>
            options.ExcludingMissingMembers());
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeUserRole = FakeUserRole.Generate();
        var updatedUserRole = new FakeUserRoleForUpdateDto().Generate();
        fakeUserRole.DomainEvents.Clear();
        
        // Act
        fakeUserRole.Update(updatedUserRole);

        // Assert
        fakeUserRole.DomainEvents.Count.Should().Be(1);
        fakeUserRole.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserRoleUpdated));
    }
}