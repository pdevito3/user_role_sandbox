namespace RecipeManagement.Domain.UserRoles.Features;

using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.Domain.UserRoles.Services;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetUserRole
{
    public class Query : IRequest<UserRoleDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public class Handler : IRequestHandler<Query, UserRoleDto>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRoleRepository userRoleRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _heimGuard = heimGuard;
        }

        public async Task<UserRoleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadUserRoles);

            var result = await _userRoleRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<UserRoleDto>(result);
        }
    }
}