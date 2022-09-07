namespace RecipeManagement.Domain.Users.DomainEvents;

public class UserCreated : DomainEvent
{
    public User User { get; set; } 
}
            