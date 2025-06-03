using Application.Common;
using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Commands.User
{
    public class LoginUserCommand : IRequest<OperationResult<LoginResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

