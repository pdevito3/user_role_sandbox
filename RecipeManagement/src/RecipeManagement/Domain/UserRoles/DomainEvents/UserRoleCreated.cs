namespace RecipeManagement.Domain.UserRoles.DomainEvents;

public class UserRoleCreated : DomainEvent
{
    public UserRole UserRole { get; set; } 
}
            