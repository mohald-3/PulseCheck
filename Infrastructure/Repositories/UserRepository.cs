using Domain.Models;
using Infrastructure.Data;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetFilteredUsersAsync(string? search, string? sort, string? order, int page, int pageSize)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(search) ||
                    u.LastName.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.Phone.Contains(search));
            }

            switch (sort?.ToLower())
            {
                case "name":
                    query = order == "asc"
                        ? query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                        : query.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName);
                    break;
                case "email":
                    query = order == "asc"
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email);
                    break;
                case "created":
                default:
                    query = order == "asc"
                        ? query.OrderBy(u => u.AccountCreationTime)
                        : query.OrderByDescending(u => u.AccountCreationTime);
                    break;
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User?> UpdateAsync(int userId, User updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null || existingUser.IsDeleted) return null;

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> SoftDeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            user.IsDeleted = true;
            user.AccountModificationTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
