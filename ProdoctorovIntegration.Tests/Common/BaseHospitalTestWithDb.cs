using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Application.DbContext;
using ProdoctorovIntegration.Domain.Client;
using Xunit;

namespace ProdoctorovIntegration.Tests.Common;

public class BaseHospitalTestWithDb : IAsyncLifetime
{
    private readonly HospitalDatabaseFixture _databaseFixture;
    protected HospitalDbContext HospitalContext = null!;
    private static bool _isDatabaseMigrated;

    protected BaseHospitalTestWithDb(HospitalDatabaseFixture databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }

    public async Task InitializeAsync()
    {
        HospitalContext = GetNewContext();

        if (!_isDatabaseMigrated)
        {
            if ((await HospitalContext.Database.GetPendingMigrationsAsync()).Any())
                await HospitalContext.Database.MigrateAsync();
        }

        _isDatabaseMigrated = true;
    }

    public async Task DisposeAsync()
    {
        HospitalContext.RemoveRange(await HospitalContext.Worker.ToListAsync());
        HospitalContext.RemoveRange(await HospitalContext.Event.ToListAsync());
        HospitalContext.RemoveRange(await HospitalContext.Client.ToListAsync());
        HospitalContext.RemoveRange(await HospitalContext.ClientContact.ToListAsync());
        HospitalContext.RemoveRange(await HospitalContext.Staff.ToListAsync());
        HospitalContext.RemoveRange(await HospitalContext.ContactType.ToListAsync());
    }

    private HospitalDbContext GetNewContext()
    {
        var dbOptions = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseNpgsql(
                _databaseFixture.Db.GetConnectionString(),
                x => x.MigrationsAssembly("MDService.Infrastructure")
                    .MinBatchSize(1)
                    .MaxBatchSize(10000))
            .Options;

        return new HospitalDbContext(dbOptions, new DbContextInitProperties(typeof(ContactTypeInfo).Assembly));
    }
}