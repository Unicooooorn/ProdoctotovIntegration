﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProdoctorovIntegration.Application.DbContext;

namespace ProdoctorovIntegration.Infrastructure.Configuration
{
    public static class ServiceProviderExtensions
    {
        public static void ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<HospitalDbContext>()!;
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
                dbContext.Database.Migrate();
        }
    }
}