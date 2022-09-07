namespace RecipeManagement.Domain.UserRoles.Features;

using RecipeManagement.Domain.UserRoles.Services;
using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddUserRole
{
    public class Command : IRequest<UserRoleDto>
    {
        public readonly UserRoleForCreationDto UserRoleToAdd;

        public Command(UserRoleForCreationDto userRoleToAdd)
        {
            UserRoleToAdd = userRoleToAdd;
        }
    }

    public class Handler : IRequestHandler<Command, UserRoleDto>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<UserRoleDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUserRole);

            var userRole = UserRole.Create(request.UserRoleToAdd);
            await _userRoleRepository.Add(userRole, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var userRoleAdded = await _userRoleRepository.GetById(userRole.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserRoleDto>(userRoleAdded);
        }
    }
}