namespace RecipeManagement.Domain.UserRoles.Dtos;

public abstract class UserRoleForManipulationDto 
{
        public Guid UserId { get; set; }
        public string Role { get; set; }
}
