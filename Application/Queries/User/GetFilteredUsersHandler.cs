using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.User
{
    public class GetFilteredUsersHandler : IRequestHandler<GetFilteredUsersQuery, OperationResult<IEnumerable<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetFilteredUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<UserDto>>> Handle(GetFilteredUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetFilteredUsersAsync(
                request.Search,
                request.Sort,
                request.Order,
                request.Page,
                request.PageSize
            );

            var mapped = _mapper.Map<IEnumerable<UserDto>>(users);
            return OperationResult<IEnumerable<UserDto>>.SuccessResult(mapped);
        }
    }
}
