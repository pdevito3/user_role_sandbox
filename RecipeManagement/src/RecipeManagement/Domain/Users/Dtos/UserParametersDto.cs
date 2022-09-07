namespace RecipeManagement.Domain.Users.Dtos;

using SharedKernel.Dtos;

public class UserParametersDto : BasePaginationParameters
{
    public string Filters { get; set; }
    public string SortOrder { get; set; }
}
