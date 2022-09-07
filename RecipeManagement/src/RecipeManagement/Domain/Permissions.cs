namespace RecipeManagement.Domain;

using System.Reflection;

public static class Permissions
{
    // Permissions marker - do not delete this comment
    public const string CanDeleteUserRole = nameof(CanDeleteUserRole);
    public const string CanUpdateUserRole = nameof(CanUpdateUserRole);
    public const string CanAddUserRole = nameof(CanAddUserRole);
    public const string CanReadUserRoles = nameof(CanReadUserRoles);
    public const string CanDeleteUser = nameof(CanDeleteUser);
    public const string CanUpdateUser = nameof(CanUpdateUser);
    public const string CanAddUser = nameof(CanAddUser);
    public const string CanReadUsers = nameof(CanReadUsers);
    public const string CanDeleteRecipe = nameof(CanDeleteRecipe);
    public const string CanUpdateRecipe = nameof(CanUpdateRecipe);
    public const string CanAddRecipe = nameof(CanAddRecipe);
    public const string CanReadRecipes = nameof(CanReadRecipes);
    public const string CanDeleteRolePermission = nameof(CanDeleteRolePermission);
    public const string CanUpdateRolePermission = nameof(CanUpdateRolePermission);
    public const string CanAddRolePermission = nameof(CanAddRolePermission);
    public const string CanReadRolePermissions = nameof(CanReadRolePermissions);
    
    public static List<string> List()
    {
        return typeof(Permissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(x => (string)x.GetRawConstantValue())
            .ToList();
    }
}
