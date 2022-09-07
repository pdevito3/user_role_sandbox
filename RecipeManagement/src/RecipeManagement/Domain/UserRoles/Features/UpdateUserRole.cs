namespace RecipeManagement.Domain.UserRoles.Features;

using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.Domain.UserRoles.Validators;
using RecipeManagement.Domain.UserRoles.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateUserRole
{
    public class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly UserRoleForUpdateDto UserRoleToUpdate;

        public Command(Guid userRole, UserRoleForUpdateDto newUserRoleData)
        {
            Id = userRole;
            UserRoleToUpdate = newUserRoleData;
        }
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateUserRole);

            var userRoleToUpdate = await _userRoleRepository.GetById(request.Id, cancellationToken: cancellationToken);

            userRoleToUpdate.Update(request.UserRoleToUpdate);
            _userRoleRepository.Update(userRoleToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}