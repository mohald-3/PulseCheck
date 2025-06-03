using Application.Common;
using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Queries.User
{
    public record GetUserByIdQuery(int UserId) : IRequest<OperationResult<UserDto>>;
}
