using Application.Common;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Friendship
{
    public class DeleteFriendshipHandler : IRequestHandler<DeleteFriendshipCommand, OperationResult<string>>
    {
        private readonly IFriendshipRepository _repository;

        public DeleteFriendshipHandler(IFriendshipRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<string>> Handle(DeleteFriendshipCommand request, CancellationToken cancellationToken)
        {
            var success = await _repository.DeleteAsync(request.UserId, request.FriendId);

            if (!success)
                return OperationResult<string>.Failure("Friendship not found or already deleted.");

            return OperationResult<string>.SuccessResult("Friendship successfully deleted.");
        }
    }
}
