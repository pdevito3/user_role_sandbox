namespace RecipeManagement.SharedTestHelpers.Fakes.User;

using AutoBogus;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;

public class FakeUser
{
    public static User Generate(UserForCreationDto userForCreationDto)
    {
        return User.Create(userForCreationDto);
    }

    public static User Generate()
    {
        return User.Create(new FakeUserForCreationDto().Generate());
    }
}