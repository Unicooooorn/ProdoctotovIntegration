using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain;
using ProdoctorovIntegration.Domain.Client;
using ProdoctorovIntegration.Domain.Worker;

namespace ProdoctorovIntegration.Infrastructure.EntityTypeConfiguration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("EVENT");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Id, "IDX_EVENT_ID")
            .IsUnique();

        builder.HasOne(x => x.Client)
            .WithOne()
            .HasForeignKey<Client>(x => x.Id);

        builder.Property(x => x.ClaimId)
            .HasColumnName("CLAIM_ID");
        builder.HasIndex(x => x.ClaimId, "IDX_EVENT_CLAIM_ID");

        builder.HasOne(x => x.Worker)
            .WithOne()
            .HasForeignKey<Worker>(x => x.Id)
            .HasConstraintName("FK_EVENT_WORKER_ID");

        builder.Property(x => x.ClientData)
            .HasColumnName("CLIENT_DATA");

        builder.Property(x => x.StartDate)
            .HasColumnName("START_DATE");

        builder.Property(x => x.Duration)
            .HasColumnName("DURATION");

        builder.Property(x => x.IsForProdoctorov)
            .HasColumnName("IS_FOR_PRODOCTOROV");

        builder.Property(x => x.RoomId)
            .HasColumnName("ROOM_ID");

        builder.Property(x => x.Note)
            .HasColumnName("NOTE");
    }
}