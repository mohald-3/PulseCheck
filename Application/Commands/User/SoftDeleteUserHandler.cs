using Application.Commands.User.Application.Commands.User;
using Application.Common;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.User
{
    public class SoftDeleteUserHandler : IRequestHandler<SoftDeleteUserCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;

        public SoftDeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<bool>> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.SoftDeleteAsync(request.UserId);

            if (!result)
                return OperationResult<bool>.Failure("User not found or already deleted.");

            return OperationResult<bool>.SuccessResult(true);
        }
    }
}
