using Application.Common;
using Application.DTOs.FriendshipDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Commands.Friendship
{
    public class CreateFriendshipHandler : IRequestHandler<CreateFriendshipCommand, OperationResult<FriendshipDto>>
    {
        private readonly IFriendshipRepository _repository;
        private readonly IMapper _mapper;

        public CreateFriendshipHandler(IFriendshipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<FriendshipDto>> Handle(CreateFriendshipCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == request.FriendId)
                return OperationResult<FriendshipDto>.Failure("You can't be friends with yourself.");

            var exists = await _repository.ExistsAsync(request.UserId, request.FriendId);
            if (exists)
                return OperationResult<FriendshipDto>.Failure("Friendship already exists.");

            var newFriend = new Domain.Models.Friendship
            {
                UserId = request.UserId,
                FriendId = request.FriendId
            };

            var created = await _repository.CreateAsync(newFriend);
            return OperationResult<FriendshipDto>.SuccessResult(_mapper.Map<FriendshipDto>(created));
        }
    }
}
