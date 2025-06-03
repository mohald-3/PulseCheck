using Application.Commands.User;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using FakeItEasy;

namespace Application.Tests.Handlers.User
{
    public class UpdateUserHandlerTests
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private UpdateUserHandler _handler;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new UpdateUserHandler(_userRepository, _mapper);
        }

        [Test]
        public async Task Handle_ValidUpdate_ReturnsSuccess()
        {
            var userId = 1;
            var existingUser = new Domain.Models.User { UserId = userId, IsDeleted = false };
            var updateDto = new UserUpdateDto { FirstName = "Updated", Phone = "99999" };

            A.CallTo(() => _userRepository.GetByIdAsync(userId)).Returns(existingUser);
            A.CallTo(() => _userRepository.UpdateAsync(userId, existingUser)).Returns(existingUser);
            A.CallTo(() => _mapper.Map<UserDto>(existingUser)).Returns(new UserDto { UserId = userId, FirstName = "Updated" });

            var command = new UpdateUserCommand(userId, updateDto);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.FirstName, Is.EqualTo("Updated"));
        }

        [Test]
        public async Task Handle_UserNotFound_ReturnsFailure()
        {
            var userId = 99;
            A.CallTo(() => _userRepository.GetByIdAsync(userId)).Returns((Domain.Models.User?)null);

            var command = new UpdateUserCommand(userId, new UserUpdateDto());
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.Success);
            Assert.That(result.Errors?.First(), Is.EqualTo("User not found"));
        }
    }
}
