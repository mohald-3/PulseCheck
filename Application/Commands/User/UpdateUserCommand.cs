using Application.Common;
using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Commands.User
{
    public class UpdateUserCommand : IRequest<OperationResult<UserDto>>
    {
        public int UserId { get; set; }
        public UserUpdateDto UpdatedData { get; set; }

        public UpdateUserCommand(int userId, UserUpdateDto updatedData)
        {
            UserId = userId;
            UpdatedData = updatedData;
        }
    }
}
