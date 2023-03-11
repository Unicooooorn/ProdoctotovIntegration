using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.ClientConfiguration;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("CLIENT");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Id, "IDX_CLIENT_ID")
            .IsUnique();

        builder.Property(x => x.FirstName)
            .HasColumnName("FIRST_NAME");
        builder.Property(x => x.PatrName)
            .HasColumnName("PATR_NAME");
        builder.Property(x => x.LastName)
            .HasColumnName("LAST_NAME");
        builder.Property(x => x.BirthDay)
            .HasColumnName("BIRTHDAY");
    }
}