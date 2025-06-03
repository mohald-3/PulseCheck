using Application.Interfaces.Services;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class RequestLogger : IRequestLogger
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RequestLogger> _logger;

        public RequestLogger(AppDbContext context, ILogger<RequestLogger> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAsync(string requestType, string requestContent, string responseContent)
        {
            var log = new RequestLog
            {
                RequestType = requestType,
                RequestContent = requestContent,
                ResponseContent = responseContent,
                Timestamp = DateTime.UtcNow
            };

            _context.RequestLogs.Add(log);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Logged request {RequestType}", requestType);
        }
    }
}
