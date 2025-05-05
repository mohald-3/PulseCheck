using Domain.Models;
using Infrastructure.Data;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CheckInRepository : ICheckInRepository
    {
        private readonly AppDbContext _context;

        public CheckInRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CheckIn> CreateAsync(CheckIn checkIn)
        {
            _context.CheckIns.Add(checkIn);
            await _context.SaveChangesAsync();
            return checkIn;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.CheckIns.FindAsync(id);
            if (entity == null) return false;

            _context.CheckIns.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CheckIn?> GetCheckInByIdAsync(int id)
        {
            return await _context.CheckIns.FindAsync(id);
        }

        public async Task<List<CheckIn>> GetUserCheckInsAsync(int userId)
        {
            return await _context.CheckIns
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.Timestamp)
                .ToListAsync();

        }
    }
}
