using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProdoctorovIntegration.Domain;

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

        builder.Property<long>("CLIENT_ID");
        builder.HasOne(x => x.Client)
            .WithOne()
            .HasForeignKey("CLIENT_ID");
        builder.HasIndex(x => x.Client.Id, "IDX_EVENT_CLIENT_ID");

        builder.Property(x => x.ClaimId)
            .HasColumnName("CLAIM_ID");
        builder.HasIndex(x => x.ClaimId, "IDX_EVENT_CLAIM_ID");

        builder.Property<long>("WORKER_ID");
        builder.HasOne(x => x.Worker)
            .WithOne()
            .HasForeignKey("WORKER_ID");
        builder.HasIndex(x => x.Worker.Id, "IDX_EVENT_CLIENT_ID");

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

        builder.Property<long>("INSERT_USER_ID");
        builder.HasOne(x => x.InsertUserId)
            .WithOne()
            .HasForeignKey("INSERT_USER_ID");
        builder.HasIndex(x => x.InsertUserId.Id, "IDX_EVENT_INSERT_USER_ID");

        builder.Property<long>("UPDATE_USER_ID");
        builder.HasOne(x => x.UpdateUserId)
            .WithOne()
            .HasForeignKey("UPDATE_USER_ID");
        builder.HasIndex(x => x.UpdateUserId.Id, "IDX_EVENT_UPDATE_USER_ID");

        builder.Property(x => x.Note)
            .HasColumnName("NOTE");
    }
}