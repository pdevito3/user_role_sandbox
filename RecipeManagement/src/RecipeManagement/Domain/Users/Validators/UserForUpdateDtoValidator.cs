namespace RecipeManagement.Domain.Users.Validators;

using RecipeManagement.Domain.Users.Dtos;
using FluentValidation;

public class UserForUpdateDtoValidator: UserForManipulationDtoValidator<UserForUpdateDto>
{
    public UserForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}