using Microsoft.Extensions.Configuration;
using Serilog;

namespace ProdoctorovIntegration.Infrastructure.Configuration;

public static class LoggingConfiguration
{
    public static ILogger GetLogger()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddDefaultConfigs()
            .Build();

        return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment ?? "Development")
            .ReadFrom.Configuration(configuration)
            .WriteTo.File("prodoctorov-integration-log-.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit:true)
            .CreateLogger();
    }

    private static IConfigurationBuilder AddDefaultConfigs(this IConfigurationBuilder configuration)
    {
        return configuration
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true,
                reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}