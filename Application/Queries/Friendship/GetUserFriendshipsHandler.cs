using Application.Common;
using Application.DTOs.FriendshipDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.Friendship
{
    public class GetUserFriendshipsHandler : IRequestHandler<GetUserFriendshipsQuery, OperationResult<IEnumerable<FriendshipDto>>>
    {
        private readonly IFriendshipRepository _repository;
        private readonly IMapper _mapper;

        public GetUserFriendshipsHandler(IFriendshipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<FriendshipDto>>> Handle(GetUserFriendshipsQuery request, CancellationToken cancellationToken)
        {
            var friends = await _repository.GetUserFriendshipsAsync(request.UserId);
            var mapped = _mapper.Map<IEnumerable<FriendshipDto>>(friends);
            return OperationResult<IEnumerable<FriendshipDto>>.SuccessResult(mapped);
        }
    }
}
