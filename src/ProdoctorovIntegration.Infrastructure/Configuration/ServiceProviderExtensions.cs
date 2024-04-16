using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Application.Interfaces;
using ProdoctorovIntegration.Application.Options;
using ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.ClientConfiguration;
using ProdoctorovIntegration.Infrastructure.Jobs;
using ProdoctorovIntegration.Infrastructure.Services;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace ProdoctorovIntegration.Infrastructure.Configuration;

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
        service.TryAddScoped<ISendScheduleService, SendScheduleService>();
        service.TryAddScoped<SendScheduleJob>();
        service.TryAddScoped<IScheduleService, ScheduleService>();
        service.TryAddScoped<IClientService, ClientService>();
        service.TryAddScoped<ISharedMutexManager, SharedMutexManager>();
    }

    public static void AddDisturbedRedisLockFactory(this IServiceCollection service, RedisLockOptions options)
    {
        service.AddSingleton<IDistributedLockFactory, RedLockFactory>(x => 
            RedLockFactory.Create(new List<RedLockEndPoint>{new DnsEndPoint(options.Host, options.Port)}));
    }
}