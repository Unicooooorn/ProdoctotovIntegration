using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ProdoctorovIntegration.Infrastructure.Configuration
{
    public static class ServiceProviderExtensions
    {
        public static void ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<>()!;
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
                dbContext.Database.Migrate();
        }
    }
}