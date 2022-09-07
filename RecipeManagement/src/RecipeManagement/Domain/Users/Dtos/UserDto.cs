namespace RecipeManagement.Domain.Users.Dtos;

public class UserDto 
{
        public Guid Id { get; set; }
        public string Sid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

}
