namespace RecipeManagement.Domain.Users;

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using DomainEvents;
using RecipeManagement.Domain.Roles;

public class UserRole : BaseEntity
{
    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("User")]
    public virtual Guid UserId { get; private set; }
    public virtual User User { get; private set; }

    public virtual Role Role { get; private set; }
    

    public static UserRole Create(Guid userId, Role role)
    {
        var newUserRole = new UserRole
        {
            UserId = userId,
            Role = role
        };

        newUserRole.QueueDomainEvent(new UserRolesUpdated(){ UserId = userId });
        
        return newUserRole;
    }
    
    protected UserRole() { } // For EF + Mocking
}