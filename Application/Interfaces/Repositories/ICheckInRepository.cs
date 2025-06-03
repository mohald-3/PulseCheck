using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface ICheckInRepository
    {
        Task<List<CheckIn>> GetUserCheckInsAsync(int userId);
        Task<CheckIn?> GetCheckInByIdAsync(int id);
        Task<bool> HasUserCheckedInTodayAsync(int userId, DateTime today);
        Task<CheckIn> CreateAsync(CheckIn checkIn);
        Task<bool> DeleteAsync(int id);

    }
}
