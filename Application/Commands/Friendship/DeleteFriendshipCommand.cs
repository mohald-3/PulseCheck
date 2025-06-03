using Application.Common;
using MediatR;

namespace Application.Commands.Friendship
{
    public class DeleteFriendshipCommand : IRequest<OperationResult<string>>
    {
        public int UserId { get; }
        public int FriendId { get; }

        public DeleteFriendshipCommand(int userId, int friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
}
