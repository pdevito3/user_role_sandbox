namespace RecipeManagement.Domain.Users.Features;

using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.Services;
using RecipeManagement.Wrappers;
using SharedKernel.Exceptions;
using RecipeManagement.Domain;
using HeimGuard;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetUserList
{
    public class Query : IRequest<PagedList<UserDto>>
    {
        public readonly UserParametersDto QueryParameters;

        public Query(UserParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public class Handler : IRequestHandler<Query, PagedList<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRepository userRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<UserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadUsers);

            var collection = _userRepository.Query();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<UserDto>();

            return await PagedList<UserDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}