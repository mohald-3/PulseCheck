using Application.Common;
using Application.DTOs.CheckInDtos;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Commands.CheckIn
{
    public class CreateCheckInHandler : IRequestHandler<CreateCheckInCommand, OperationResult<CheckInDto>>
    {
        private readonly ICheckInRepository _checkInRepo;
        private readonly IMapper _mapper;

        public CreateCheckInHandler(ICheckInRepository checkInRepo, IMapper mapper)
        {
            _checkInRepo = checkInRepo;
            _mapper = mapper;
        }

        public async Task<OperationResult<CheckInDto>> Handle(CreateCheckInCommand request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // Check if the user has already checked in today
            var existingCheckIns = await _checkInRepo.GetUserCheckInsAsync(request.UserId);
            if (existingCheckIns.Any(ci => ci.Timestamp.Date == today))
            {
                return OperationResult<CheckInDto>.Failure("You have already checked in today.");
            }

            var checkIn = new Domain.Models.CheckIn
            {
                UserId = request.UserId,
                Timestamp = DateTime.UtcNow
            };

            var created = await _checkInRepo.CreateAsync(checkIn);
            var result = _mapper.Map<CheckInDto>(created);

            return OperationResult<CheckInDto>.SuccessResult(result);
        }
    }
}
