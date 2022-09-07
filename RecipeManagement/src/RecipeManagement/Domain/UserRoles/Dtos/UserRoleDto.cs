namespace RecipeManagement.Domain.UserRoles.Dtos;

public class UserRoleDto 
{
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
}
