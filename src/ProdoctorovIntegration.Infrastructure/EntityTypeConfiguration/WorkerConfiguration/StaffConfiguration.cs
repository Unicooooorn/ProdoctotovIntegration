using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.WorkerConfiguration;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("STAFF");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Id, "IDX_STAFF_ID")
            .IsUnique();

        builder.Property(x => x.Department)
            .HasColumnName("DEPARTMENT");
        builder.Property(x => x.Speciality)
            .HasColumnName("SPECILITY");
    }
}