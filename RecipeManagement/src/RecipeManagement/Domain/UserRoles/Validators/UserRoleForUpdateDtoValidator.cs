namespace RecipeManagement.Domain.UserRoles.Validators;

using RecipeManagement.Domain.UserRoles.Dtos;
using FluentValidation;

public class UserRoleForUpdateDtoValidator: UserRoleForManipulationDtoValidator<UserRoleForUpdateDto>
{
    public UserRoleForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}