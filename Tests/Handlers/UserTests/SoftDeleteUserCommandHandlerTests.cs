
using Application.Commands.User;
using Application.Commands.User.Application.Commands.User;
using Application.Interfaces.Repositories;
using AutoMapper;
using FakeItEasy;

namespace Application.Tests.Handlers.User
{
    public class SoftDeleteUserCommandHandlerTests
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private SoftDeleteUserHandler _handler;

        [SetUp]
        public void Setup()
        {
            _userRepository = A.Fake<IUserRepository>();
            _mapper = A.Fake<IMapper>();
            _handler = new SoftDeleteUserHandler(_userRepository);
        }

        [Test]
        public async Task Handle_ValidUserId_ReturnsSuccessResult()
        {
            A.CallTo(() => _userRepository.SoftDeleteAsync(1)).Returns(true);

            var command = new SoftDeleteUserCommand(1);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.True);
        }

        [Test]
        public async Task Handle_UserNotFound_ReturnsFailure()
        {
            A.CallTo(() => _userRepository.GetByIdAsync(999)).Returns(Task.FromResult<Domain.Models.User>(null));
            var command = new SoftDeleteUserCommand(999);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("User not found or already deleted.", result.Errors?[0]);
        }
    }
}
