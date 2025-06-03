using Application.Common;
using Application.DTOs.FriendshipDtos;
using MediatR;

namespace Application.Commands.Friendship
{
    public class CreateFriendshipCommand : IRequest<OperationResult<FriendshipDto>>
    {
        public int UserId { get; }
        public int FriendId { get; }

        public CreateFriendshipCommand(int userId, int friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }
}
