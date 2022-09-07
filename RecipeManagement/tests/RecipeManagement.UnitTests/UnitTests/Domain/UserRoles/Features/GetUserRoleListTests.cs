namespace RecipeManagement.UnitTests.UnitTests.Domain.UserRoles.Features;

using RecipeManagement.SharedTestHelpers.Fakes.UserRole;
using RecipeManagement.Domain.UserRoles;
using RecipeManagement.Domain.UserRoles.Dtos;
using RecipeManagement.Domain.UserRoles.Mappings;
using RecipeManagement.Domain.UserRoles.Features;
using RecipeManagement.Domain.UserRoles.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetUserRoleListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IUserRoleRepository> _userRoleRepository;
      private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetUserRoleListTests()
    {
        _userRoleRepository = new Mock<IUserRoleRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_userRole()
    {
        //Arrange
        var fakeUserRoleOne = FakeUserRole.Generate();
        var fakeUserRoleTwo = FakeUserRole.Generate();
        var fakeUserRoleThree = FakeUserRole.Generate();
        var userRole = new List<UserRole>();
        userRole.Add(fakeUserRoleOne);
        userRole.Add(fakeUserRoleTwo);
        userRole.Add(fakeUserRoleThree);
        var mockDbData = userRole.AsQueryable().BuildMock();
        
        var queryParameters = new UserRoleParametersDto() { PageSize = 1, PageNumber = 2 };

        _userRoleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetUserRoleList.Query(queryParameters);
        var handler = new GetUserRoleList.Handler(_userRoleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }
}