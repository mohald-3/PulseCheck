using Application.Commands.User;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using FakeItEasy;

namespace Tests.Handlers.UserTests
{
    public class LoginUserHandlerTests
    {
        private IUserRepository _userRepo;
        private ITokenService _tokenService;
        private IMapper _mapper;
        private LoginUserHandler _handler;

        [SetUp]
        public void Setup()
        {
            _userRepo = A.Fake<IUserRepository>();
            _tokenService = A.Fake<ITokenService>();
            _mapper = A.Fake<IMapper>();
            _handler = new LoginUserHandler(_userRepo, _mapper, _tokenService);
        }

        [Test]
        public async Task Handle_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "test@example.com",
                Password = "Password123"
            };

            var user = new User
            {
                UserId = 1,
                Email = command.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
                FirstName = "Test",
                LastName = "User"
            };

            A.CallTo(() => _userRepo.GetByEmailAsync(command.Email)).Returns(user);
            A.CallTo(() => _userRepo.UpdateAsync(user.UserId, user)).Returns(user);
            A.CallTo(() => _tokenService.CreateToken(user)).Returns("mock-token");
            A.CallTo(() => _mapper.Map<UserDto>(user)).Returns(new UserDto { Email = user.Email });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("mock-token", result.Data.Token);
        }

        [Test]
        public async Task Handle_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var command = new LoginUserCommand
            {
                Email = "wrong@example.com",
                Password = "wrongpass"
            };

            A.CallTo(() => _userRepo.GetByEmailAsync(command.Email)).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
            Assert.That(result.Errors, Does.Contain("Invalid credentials."));
        }
    }
}
