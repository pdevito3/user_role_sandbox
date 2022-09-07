namespace RecipeManagement.UnitTests.UnitTests.Domain.Users.Features;

using RecipeManagement.SharedTestHelpers.Fakes.User;
using RecipeManagement.Domain.Users;
using RecipeManagement.Domain.Users.Dtos;
using RecipeManagement.Domain.Users.Mappings;
using RecipeManagement.Domain.Users.Features;
using RecipeManagement.Domain.Users.Services;
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

public class GetUserListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IUserRepository> _userRepository;
      private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetUserListTests()
    {
        _userRepository = new Mock<IUserRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_user()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate();
        var fakeUserTwo = FakeUser.Generate();
        var fakeUserThree = FakeUser.Generate();
        var user = new List<User>();
        user.Add(fakeUserOne);
        user.Add(fakeUserTwo);
        user.Add(fakeUserThree);
        var mockDbData = user.AsQueryable().BuildMock();
        
        var queryParameters = new UserParametersDto() { PageSize = 1, PageNumber = 2 };

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_user_list_using_Sid()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Sid, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Sid, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { Filters = $"Sid == {fakeUserTwo.Sid}" };

        var userList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = userList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_user_list_using_FirstName()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.FirstName, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.FirstName, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { Filters = $"FirstName == {fakeUserTwo.FirstName}" };

        var userList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = userList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_user_list_using_LastName()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.LastName, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.LastName, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { Filters = $"LastName == {fakeUserTwo.LastName}" };

        var userList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = userList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_user_list_using_Email()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Email, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Email, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { Filters = $"Email == {fakeUserTwo.Email}" };

        var userList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = userList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_user_list_using_Username()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Username, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Username, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { Filters = $"Username == {fakeUserTwo.Username}" };

        var userList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = userList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_user_by_Sid()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Sid, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Sid, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { SortOrder = "-Sid" };

        var UserList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = UserList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_user_by_FirstName()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.FirstName, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.FirstName, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { SortOrder = "-FirstName" };

        var UserList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = UserList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_user_by_LastName()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.LastName, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.LastName, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { SortOrder = "-LastName" };

        var UserList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = UserList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_user_by_Email()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Email, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Email, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { SortOrder = "-Email" };

        var UserList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = UserList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_user_by_Username()
    {
        //Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Username, _ => "alpha")
            .Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto()
            .RuleFor(u => u.Username, _ => "bravo")
            .Generate());
        var queryParameters = new UserParametersDto() { SortOrder = "-Username" };

        var UserList = new List<User>() { fakeUserOne, fakeUserTwo };
        var mockDbData = UserList.AsQueryable().BuildMock();

        _userRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetUserList.Query(queryParameters);
        var handler = new GetUserList.Handler(_userRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeUserOne, options =>
                options.ExcludingMissingMembers());
    }
}