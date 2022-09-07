using Microsoft.EntityFrameworkCore;

namespace RecipeManagement.Domain.UserRoles.Services;

using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Databases;
using RecipeManagement.Services;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    IEnumerable<string> GetRolesByUserSid(string userSid);
}

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly RecipesDbContext _dbContext;

    public UserRoleRepository(RecipesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<string> GetRolesByUserSid(string userSid)
    {
        return _dbContext.UserRoles
            .Include(x => x.User)
            .Where(x => x.User.Sid == userSid)
            .Select(x => x.Role.Value);
    }
}
