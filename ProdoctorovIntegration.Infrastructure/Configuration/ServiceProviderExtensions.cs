﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Services;
using ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.ClientConfiguration;
using ProdoctorovIntegration.Infrastructure.Jobs;
using ProdoctorovIntegration.Infrastructure.Services;

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

        public static void AddEfCore(this IServiceCollection services,
            string dbConnectionString)
        {
            services
                .AddSingleton(new DbContextInitProperties(typeof(ClientConfiguration).Assembly))
                .AddDbContext<HospitalDbContext>(
                    options => options.UseNpgsql(
                        dbConnectionString,
                        x => x.MigrationsAssembly("ProdoctorovIntegration.Infrastructure")));
        }

        public static void AddServices(this IServiceCollection service)
        {
            service.AddHttpClient();
            service.AddScoped<ISendScheduleService, SendScheduleService>();
            service.AddScoped<SendScheduleJob>();
            service.AddScoped<IScheduleService, ScheduleService>();
            service.AddScoped<IClientService, ClientService>();
        }
    }
}