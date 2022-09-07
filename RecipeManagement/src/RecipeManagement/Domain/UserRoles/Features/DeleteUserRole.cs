namespace RecipeManagement.Domain.UserRoles.Features;

using RecipeManagement.Domain.UserRoles.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MediatR;

public static class DeleteUserRole
{
    public class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid userRole)
        {
            Id = userRole;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteUserRole);

            var recordToDelete = await _userRoleRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _userRoleRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}