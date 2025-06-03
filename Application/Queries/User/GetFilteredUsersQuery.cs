using Application.Common;
using Application.DTOs.UserDtos;
using MediatR;

namespace Application.Queries.User
{
    public class GetFilteredUsersQuery : IRequest<OperationResult<IEnumerable<UserDto>>>
    {
        public string? Search { get; set; }
        public string? Sort { get; set; } = "created";
        public string? Order { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
