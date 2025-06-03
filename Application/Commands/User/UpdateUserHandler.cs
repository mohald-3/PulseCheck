using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Commands.User
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, OperationResult<UserDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null || user.IsDeleted)
                return OperationResult<UserDto>.Failure("User not found");

            var data = request.UpdatedData;

            // Apply changes if provided
            if (!string.IsNullOrWhiteSpace(data.FirstName))
                user.FirstName = data.FirstName;

            if (!string.IsNullOrWhiteSpace(data.LastName))
                user.LastName = data.LastName;

            if (!string.IsNullOrWhiteSpace(data.Phone))
                user.Phone = data.Phone;

            if (!string.IsNullOrWhiteSpace(data.EmergencyContactName))
                user.EmergencyContactName = data.EmergencyContactName;

            if (!string.IsNullOrWhiteSpace(data.EmergencyContactPhone))
                user.EmergencyContactPhone = data.EmergencyContactPhone;

            user.AccountModificationTime = DateTime.UtcNow;

            var updated = await _userRepo.UpdateAsync(user.UserId, user);
            return OperationResult<UserDto>.SuccessResult(_mapper.Map<UserDto>(updated));
        }
    }
}
