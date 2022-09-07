namespace RecipeManagement.SharedTestHelpers.Fakes.UserRole;

using AutoBogus;
using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Domain.UserRoles.Dtos;

public class FakeUserRole
{
    public static UserRole Generate(UserRoleForCreationDto userRoleForCreationDto)
    {
        return UserRole.Create(userRoleForCreationDto);
    }

    public static UserRole Generate()
    {
        return UserRole.Create(new FakeUserRoleForCreationDto().Generate());
    }
}