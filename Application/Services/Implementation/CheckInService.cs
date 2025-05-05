using Application.DTOs.CheckInDtos;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;

namespace Application.Services.Implementation
{
    public class CheckInService : ICheckInService
    {
        private readonly ICheckInRepository _checkInRepo;
        private readonly IMapper _mapper;

        public CheckInService(ICheckInRepository checkInRepo, IMapper mapper)
        {
            _checkInRepo = checkInRepo;
            _mapper = mapper;
        }

        public async Task<CheckInDto> CreateCheckInAsync(CheckInCreateDto checkInData)
        {
            // Map the DTO to a CheckIn entity
            var newCheckIn = _mapper.Map<CheckIn>(checkInData);
            newCheckIn.Timestamp = DateTime.UtcNow;


            var createdCheckIn = await _checkInRepo.CreateAsync(newCheckIn);

            return _mapper.Map<CheckInDto>(createdCheckIn);
        }

        public async Task<CheckInDto?> GetCheckInByIdAsync(int checkInId)
        {
            var checkIn = await _checkInRepo.GetCheckInByIdAsync(checkInId);
            return checkIn == null ? null : _mapper.Map<CheckInDto>(checkIn);
        }

        public async Task<IEnumerable<CheckInDto>> GetCheckInsByUserIdAsync(int userId)
        {
            var checkIns = await _checkInRepo.GetUserCheckInsAsync(userId);
            return _mapper.Map<IEnumerable<CheckInDto>>(checkIns);
        }

        public async Task<bool> DeleteCheckInAsync(int checkInId)
        {
            return await _checkInRepo.DeleteAsync(checkInId);
        }
    }
}
