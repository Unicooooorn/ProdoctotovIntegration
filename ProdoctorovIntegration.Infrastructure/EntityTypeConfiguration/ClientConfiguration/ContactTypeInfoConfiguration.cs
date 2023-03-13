using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.ClientConfiguration;

public class ContactTypeInfoConfiguration : IEntityTypeConfiguration<ContactTypeInfo>
{
    public void Configure(EntityTypeBuilder<ContactTypeInfo> builder)
    {
        builder.ToTable("CONTACT_TYPE");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Id, "IDX_CONTACT_TYPE_INFO")
            .IsUnique();

        builder.Property(x => x.Code)
            .HasColumnName("CODE");
        builder.Property(x => x.Name)
            .HasColumnName("NAME");
    }
}