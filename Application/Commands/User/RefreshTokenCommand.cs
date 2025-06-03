using Application.Common;
using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Commands.User
{
    public class RefreshTokenCommand : IRequest<OperationResult<LoginResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
