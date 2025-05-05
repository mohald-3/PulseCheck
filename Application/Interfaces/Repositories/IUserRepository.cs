using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetFilteredUsersAsync(string? search, string? sort, string? order, int page, int pageSize);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int userId, User updatedUser);
        Task<bool> SoftDeleteAsync(int userId);
    }
}
