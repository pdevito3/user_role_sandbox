namespace RecipeManagement.Domain.Users.Dtos;

public abstract class UserForManipulationDto 
{
        public string Sid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

}
