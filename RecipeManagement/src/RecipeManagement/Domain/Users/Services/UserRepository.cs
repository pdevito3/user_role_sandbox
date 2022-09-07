namespace RecipeManagement.Domain.Users.Services;

using RecipeManagement.Domain.Users;
using RecipeManagement.Databases;
using RecipeManagement.Services;

public interface IUserRepository : IGenericRepository<User>
{
}

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly RecipesDbContext _dbContext;

    public UserRepository(RecipesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
