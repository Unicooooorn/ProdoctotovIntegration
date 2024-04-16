using Ductus.FluentDocker;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Npgsql;
using System.Net.Sockets;
using System.Transactions;
using Polly;

namespace ProdoctorovIntegration.Tests.Common;

public class TestingPostgreSqlContainer : IDisposable
{
    private const int ContainerPort = 5432;
    private const string DockerImage = "postgres:14";
    private const string ContainerName = "test-database";
    private const string Host = "127.0.0.1";
    private const string User = "someUser";
    private const string Password = "somePassword";
    private const string DbName = "someDbName";

    private readonly IContainerService _containerService;

    private bool _dbReady;

    public TestingPostgreSqlContainer(string appName)
    {
        _containerService = GetOrCreateContainerService(appName);
    }

    private static IContainerService GetOrCreateContainerService(string appName)
    {
        var containers = Fd.Discover()[0].GetContainers();

        var fullContainerName = $"{ContainerName}-{appName}";
        var existingContainer = containers
            .FirstOrDefault(x => $"{x.Image.Name}:{x.Image.Tag}" == DockerImage && x.Name == fullContainerName);
        if (existingContainer is not null)
        {
            if (existingContainer.State is ServiceRunningState.Paused or ServiceRunningState.Stopped)
            {
                existingContainer.Start();
                existingContainer.WaitForRunning();
            }

            return existingContainer;
        }

        var builder = Fd.UseContainer();

        var service = builder
            .UseImage(DockerImage)
            .ReuseIfExists()
            .WithName(fullContainerName)
            .Command("postgres -N 2000")
            .WithEnvironment($"POSTGRES_PASSWORD={Password}", $"POSTGRES_USER={User}", $"POSTGRES_DB={DbName}")
            .ExposePort(ContainerPort)
            .Build()
            .Start();

        service
            .ToHostExposedEndpoint($"{ContainerPort}/tcp")
            ?.WaitForPort();

        return service;
    }

    public string GetConnectionString()
    {
        return new NpgsqlConnectionStringBuilder
            {
                Host = Host,
                Port = GetExposedPortFor(ContainerPort),
                Database = DbName,
                Username = User,
                Password = Password,
                Pooling = false
            }
            .ConnectionString;
    }

    private int GetExposedPortFor(int port)
    {
        var ipEndpoint = _containerService.ToHostExposedEndpoint($"{port}/tcp");

        if (ipEndpoint == null)
            throw new InvalidOperationException($"port {port} of container {_containerService.Name} was not exposed");

        return ipEndpoint.Port;
    }

    public void Start()
    {
        if (_dbReady)
            return;

        EnsureDbReady();

        _dbReady = true;
    }

    private void EnsureDbReady()
    {
        const int timeoutInSeconds = 10;
        const int sleepDurationInSeconds = 1;

        var policyResult = Policy
            .Handle<NpgsqlException>()
            .Or<SocketException>()
            .WaitAndRetry(
                new[]
                {
                    TimeSpan.FromSeconds(sleepDurationInSeconds),
                    TimeSpan.FromSeconds(sleepDurationInSeconds),
                    TimeSpan.FromSeconds(sleepDurationInSeconds)
                })
            .Wrap(Policy.Timeout(TimeSpan.FromSeconds(timeoutInSeconds)))
            .ExecuteAndCapture(
                () =>
                {
                    using var connection = new NpgsqlConnection(GetConnectionString());

                    using (new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        connection.Open();
                        connection.Close();
                    }
                });

        if (policyResult.Outcome == OutcomeType.Failure)
        {
            throw policyResult.FinalException;
        }
    }

    public void Dispose()
    {
        _containerService.Dispose();
    }
}