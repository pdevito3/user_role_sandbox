namespace RecipeManagement.Domain.Roles.Mappings;

using Mapster;
using Roles;

public class RoleMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, Role>()
            .MapWith(value => new Role(value));
        config.NewConfig<Role, string>()
            .MapWith(role => role.Value);
    }
}