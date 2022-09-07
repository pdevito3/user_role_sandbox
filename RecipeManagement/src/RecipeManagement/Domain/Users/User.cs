namespace RecipeManagement.Domain.Users;

using SharedKernel.Exceptions;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.Validators;
using RecipeManagement.Domain.Users.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using RecipeManagement.Domain.UserRoles;


public class User : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Sid { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string FirstName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string LastName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Email { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Username { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<UserRole> UserRoles { get; private set; }


    public static User Create(UserForCreationDto userForCreationDto)
    {
        new UserForCreationDtoValidator().ValidateAndThrow(userForCreationDto);

        var newUser = new User();

        newUser.Sid = userForCreationDto.Sid;
        newUser.FirstName = userForCreationDto.FirstName;
        newUser.LastName = userForCreationDto.LastName;
        newUser.Email = userForCreationDto.Email;
        newUser.Username = userForCreationDto.Username;

        newUser.QueueDomainEvent(new UserCreated(){ User = newUser });
        
        return newUser;
    }

    public void Update(UserForUpdateDto userForUpdateDto)
    {
        new UserForUpdateDtoValidator().ValidateAndThrow(userForUpdateDto);

        Sid = userForUpdateDto.Sid;
        FirstName = userForUpdateDto.FirstName;
        LastName = userForUpdateDto.LastName;
        Email = userForUpdateDto.Email;
        Username = userForUpdateDto.Username;

        QueueDomainEvent(new UserUpdated(){ Id = Id });
    }
    
    protected User() { } // For EF + Mocking
}