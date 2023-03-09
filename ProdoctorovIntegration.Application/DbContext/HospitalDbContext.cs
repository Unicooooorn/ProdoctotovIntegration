using Microsoft.EntityFrameworkCore;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Application.DbContext;

public class HospitalDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public HospitalDbContext(
        DbContextOptions<HospitalDbContext> options) : base(options)
    { }

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
    }
}