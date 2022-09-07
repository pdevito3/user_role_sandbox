namespace RecipeManagement.Domain.Users.DomainEvents;

public class UserUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            