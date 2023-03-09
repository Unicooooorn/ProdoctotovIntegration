using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.WorkerConfiguration;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("WORKER");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Id, "IDX_WORKER_ID")
            .IsUnique();

        builder.Property<long>("STAFF_ID");
        builder.HasOne(x => x.Staff)
            .WithOne()
            .HasForeignKey("STAFF_ID");

        builder.Property(x => x.FirstName)
            .HasColumnName("FIRST_NAME");
        builder.Property(x => x.PatrName)
            .HasColumnName("PATR_NAME");
        builder.Property(x => x.SurName)
            .HasColumnName("SUR_NAME");
    }
}