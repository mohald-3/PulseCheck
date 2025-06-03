using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Application.Queries.User;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;

namespace Tests.Handlers.UserTests
{
    public class GetUserByIdHandlerTests
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private GetUserByIdHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userRepository = A.Fake<IUserRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new GetUserByIdHandler(_userRepository, _mapper);
        }

        [Test]
        public async Task Handle_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new Domain.Models.User { UserId = userId, FirstName = "John", LastName = "Doe" };
            var userDto = new UserDto { UserId = userId, FirstName = "John", LastName = "Doe" };

            A.CallTo(() => _userRepository.GetByIdAsync(userId)).Returns(Task.FromResult(user));
            A.CallTo(() => _mapper.Map<UserDto>(user)).Returns(userDto);

            var query = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(userDto);
        }

        [Test]
        public async Task Handle_ReturnsFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 999;
            A.CallTo(() => _userRepository.GetByIdAsync(userId)).Returns(Task.FromResult<Domain.Models.User>(null));

            var query = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found.");
            result.Data.Should().BeNull();
        }
    }
}
