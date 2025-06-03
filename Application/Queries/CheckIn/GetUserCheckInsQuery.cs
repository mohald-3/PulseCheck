using Application.Common;
using Application.DTOs.CheckInDtos;
using MediatR;

namespace Application.Queries.CheckIn
{
    public class GetUserCheckInsQuery : IRequest<OperationResult<IEnumerable<CheckInDto>>>
    {
        public int UserId { get; }

        public GetUserCheckInsQuery(int userId)
        {
            UserId = userId;
        }
    }
}
