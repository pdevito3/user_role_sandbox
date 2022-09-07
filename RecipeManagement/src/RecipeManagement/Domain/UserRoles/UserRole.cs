namespace RecipeManagement.Domain.UserRoles;

using SharedKernel.Exceptions;
using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.Domain.UserRoles.Validators;
using RecipeManagement.Domain.UserRoles.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.Users;


public class UserRole : BaseEntity
{
    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("User")]
    public virtual Guid UserId { get; private set; }
    public virtual User User { get; private set; }

    private RoleEnum _role;
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Role
    {
        get => _role.Name;
        private set
        {
            if (!RoleEnum.TryFromName(value, true, out var parsed))
                throw new InvalidSmartEnumPropertyName(nameof(Role), value);

            _role = parsed;
        }
    }


    public static UserRole Create(UserRoleForCreationDto userRoleForCreationDto)
    {
        new UserRoleForCreationDtoValidator().ValidateAndThrow(userRoleForCreationDto);

        var newUserRole = new UserRole();

        newUserRole.UserId = userRoleForCreationDto.UserId;
        newUserRole.Role = userRoleForCreationDto.Role;

        newUserRole.QueueDomainEvent(new UserRoleCreated(){ UserRole = newUserRole });
        
        return newUserRole;
    }

    public void Update(UserRoleForUpdateDto userRoleForUpdateDto)
    {
        new UserRoleForUpdateDtoValidator().ValidateAndThrow(userRoleForUpdateDto);

        UserId = userRoleForUpdateDto.UserId;
        Role = userRoleForUpdateDto.Role;

        QueueDomainEvent(new UserRoleUpdated(){ Id = Id });
    }
    
    protected UserRole() { } // For EF + Mocking
}