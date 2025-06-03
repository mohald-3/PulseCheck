using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.User
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, OperationResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return OperationResult<UserDto>.Failure("User not found.");

            var dto = _mapper.Map<UserDto>(user);
            return OperationResult<UserDto>.SuccessResult(dto);
        }
    }
}
