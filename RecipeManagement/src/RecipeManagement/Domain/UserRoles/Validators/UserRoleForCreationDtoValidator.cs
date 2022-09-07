namespace RecipeManagement.Domain.UserRoles.Validators;

using RecipeManagement.Domain.UserRoles.Dtos;
using FluentValidation;

public class UserRoleForCreationDtoValidator: UserRoleForManipulationDtoValidator<UserRoleForCreationDto>
{
    public UserRoleForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}