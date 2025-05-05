using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class AppDbInitializer
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!dbContext.Users.Any())
            {
                FakeDataSeeder.SeedAsync(dbContext).GetAwaiter().GetResult();
            }
        }
    }
}
