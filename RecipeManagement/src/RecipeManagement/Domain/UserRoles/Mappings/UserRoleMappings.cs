namespace RecipeManagement.Domain.UserRoles.Mappings;

using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.Domain.UserRoles;
using Mapster;

public class UserRoleMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserRoleDto, UserRole>()
            .TwoWays();
        config.NewConfig<UserRoleForCreationDto, UserRole>()
            .TwoWays();
        config.NewConfig<UserRoleForUpdateDto, UserRole>()
            .TwoWays();
    }
}