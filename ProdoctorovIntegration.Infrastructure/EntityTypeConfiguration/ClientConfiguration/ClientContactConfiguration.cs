using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain.Client;

namespace ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration.ClientConfiguration;

public class ClientContactConfiguration : IEntityTypeConfiguration<ClientContact>
{
    public void Configure(EntityTypeBuilder<ClientContact> builder)
    {
        builder.ToTable("CLIENT_CONTACT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Id, "IDX_CLIENT_CONTACT_ID")
            .IsUnique();

        builder.Property(x => x.ContactOnlyDigits)
            .HasColumnName("CONTACT_ONLY_DIGITS");

        builder.HasOne(x => x.ContactInfoType)
            .WithOne()
            .HasForeignKey<ContactTypeInfo>(x => x.Id);

        builder.HasOne(x => x.Client)
            .WithOne()
            .HasForeignKey<Client>(x => x.Id);
    }
}