using Application.Common;
using Application.DTOs.CheckInDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.CheckIn
{
    public class GetUserCheckInsHandler : IRequestHandler<GetUserCheckInsQuery, OperationResult<IEnumerable<CheckInDto>>>
    {
        private readonly ICheckInRepository _checkInRepo;
        private readonly IMapper _mapper;

        public GetUserCheckInsHandler(ICheckInRepository checkInRepo, IMapper mapper)
        {
            _checkInRepo = checkInRepo;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<CheckInDto>>> Handle(GetUserCheckInsQuery request, CancellationToken cancellationToken)
        {
            var checkIns = await _checkInRepo.GetUserCheckInsAsync(request.UserId);

            if (checkIns == null || checkIns.Count == 0)
                return OperationResult<IEnumerable<CheckInDto>>.Failure("No check-ins found.");

            var mapped = _mapper.Map<IEnumerable<CheckInDto>>(checkIns);
            return OperationResult<IEnumerable<CheckInDto>>.SuccessResult(mapped);
        }
    }

}
