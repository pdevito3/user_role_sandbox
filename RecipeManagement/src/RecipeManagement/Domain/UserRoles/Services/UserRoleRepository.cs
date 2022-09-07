namespace RecipeManagement.Domain.UserRoles.Services;

using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Databases;
using RecipeManagement.Services;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
}

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly RecipesDbContext _dbContext;

    public UserRoleRepository(RecipesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
