namespace RecipeManagement.Domain.UserRoles.Dtos;

using SharedKernel.Dtos;

public class UserRoleParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
