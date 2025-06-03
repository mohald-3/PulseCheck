using Application.Common;
using Application.DTOs.CheckInDtos;
using MediatR;

namespace Application.Commands.CheckIn
{
    public class CreateCheckInCommand : IRequest<OperationResult<CheckInDto>>
    {
        public int UserId { get; set; }

        public CreateCheckInCommand(int userId)
        {
            UserId = userId;
        }
    }
}
