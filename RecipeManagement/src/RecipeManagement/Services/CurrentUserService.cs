namespace RecipeManagement.Services;

using System.Security.Claims;

public interface ICurrentUserService : IRecipeManagementService
{
    string? UserId { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
    string? Username { get; }
    ClaimsPrincipal? User { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public string? FirstName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);
    public string? LastName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname);
    public string? Username => _httpContextAccessor.HttpContext
        ?.User
        ?.Claims
        ?.FirstOrDefault(x => x.ValueType is "preferred_username" or "username")
        ?.Value;
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
}