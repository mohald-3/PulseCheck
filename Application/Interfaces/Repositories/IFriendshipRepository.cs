using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        Task<Friendship> CreateAsync(Friendship friendship);
        Task<IEnumerable<Friendship>> GetUserFriendshipsAsync(int userId);
        Task<bool> DeleteAsync(int userId, int friendId);
        Task<bool> ExistsAsync(int userId, int friendId);
    }
}
