using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Application.DbContext;

public class HospitalDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly DbContextInitProperties _initProperties;

    public HospitalDbContext(
        DbContextOptions<HospitalDbContext> options, DbContextInitProperties initProperties) : base(options)
    {
        _initProperties = initProperties;
    }

    public DbSet<ContactTypeInfo> Type => Set<ContactTypeInfo>();

    public DbSet<Client> Client => Set<Client>();

    public DbSet<ClientContact> ClientContact => Set<ClientContact>();

    public DbSet<Staff> Staff => Set<Staff>();

    public DbSet<Worker> Worker => Set<Worker>();

    public DbSet<Event> Event => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("HOSPITAL");

        modelBuilder.UseSerialColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(_initProperties.MappingAssembly);
    }
}