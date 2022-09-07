namespace RecipeManagement.Domain.Users.Validators;

using RecipeManagement.Domain.Users.Dtos;
using FluentValidation;

public class UserForCreationDtoValidator: UserForManipulationDtoValidator<UserForCreationDto>
{
    public UserForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}