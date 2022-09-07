namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Domain.Users.Services;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddUser
{
    public class Command : IRequest<UserDto>
    {
        public readonly UserForCreationDto UserToAdd;

        public Command(UserForCreationDto userToAdd)
        {
            UserToAdd = userToAdd;
        }
    }

    public class Handler : IRequestHandler<Command, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddUser);

            var user = User.Create(request.UserToAdd);
            await _userRepository.Add(user, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var userAdded = await _userRepository.GetById(user.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserDto>(userAdded);
        }
    }
}