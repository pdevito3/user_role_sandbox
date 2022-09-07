namespace RecipeManagement.Domain.UserRoles;

using Ardalis.SmartEnum;

public abstract class RoleEnum : SmartEnum<RoleEnum>
{
    public static readonly RoleEnum User = new UserType();
    public static readonly RoleEnum SuperAdmin = new SuperAdminType();

    protected RoleEnum(string name, int value) : base(name, value)
    {
    }

    private class UserType : RoleEnum
    {
        public UserType() : base("User", 0)
        {
        }
    }

    private class SuperAdminType : RoleEnum
    {
        public SuperAdminType() : base("Super Admin", 1)
        {
        }
    }
}