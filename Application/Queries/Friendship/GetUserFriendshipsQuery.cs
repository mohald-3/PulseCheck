using Application.Common;
using Application.DTOs.FriendshipDtos;
using MediatR;

namespace Application.Queries.Friendship
{
    public class GetUserFriendshipsQuery : IRequest<OperationResult<IEnumerable<FriendshipDto>>>
    {
        public int UserId { get; }

        public GetUserFriendshipsQuery(int userId)
        {
            UserId = userId;
        }
    }
}
