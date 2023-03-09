﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProdoctorovIntegration.Application.DbContext;

#nullable disable

namespace ProdoctorovIntegration.Infrastructure.Migrations
{
    [DbContext(typeof(HospitalDbContext))]
    [Migration("20230309174230_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("HOSPITAL")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Client.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("BIRTHDAY");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("FIRST_NAME");

                    b.Property<string>("PatrName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PATR_NAME");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("SUR_NAME");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IDX_CLIENT_ID")
                        .IsUnique();

                    b.ToTable("CLIENT", "HOSPITAL");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Client.ClientContact", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<long>("Id"));

                    b.Property<long>("ContactOnlyDigits")
                        .HasColumnType("bigint")
                        .HasColumnName("CONTACT_ONLY_DIGITS");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IDX_CLIENT_CONTACT_ID")
                        .IsUnique();

                    b.ToTable("CLIENT_CONTACT", "HOSPITAL");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Client.ContactTypeInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    b.Property<long>("Code")
                        .HasColumnType("bigint")
                        .HasColumnName("CODE");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("NAME");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IDX_CONTACT_TYPE_INFO_ID")
                        .IsUnique();

                    b.ToTable("CONTACT_TYPE", "HOSPITAL");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<long>("Id"));

                    b.Property<Guid?>("ClaimId")
                        .HasColumnType("uuid")
                        .HasColumnName("CLAIM_ID");

                    b.Property<string>("ClientData")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("CLIENT_DATA");

                    b.Property<long>("Duration")
                        .HasColumnType("bigint")
                        .HasColumnName("DURATION");

                    b.Property<long>("InsertUserId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsForProdoctorov")
                        .HasColumnType("boolean")
                        .HasColumnName("IS_FOR_PRODOCTOROV");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("NOTE");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint")
                        .HasColumnName("ROOM_ID");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("START_DATE");

                    b.Property<long>("WorkerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InsertUserId");

                    b.HasIndex("WorkerId");

                    b.HasIndex(new[] { "ClaimId" }, "IDX_EVENT_CLAIM_ID");

                    b.HasIndex(new[] { "Id" }, "IDX_EVENT_ID")
                        .IsUnique();

                    b.ToTable("EVENT", "HOSPITAL");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Worker.Staff", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("DEPARTMENT");

                    b.Property<string>("Speciality")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("SPECILITY");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IDX_STAFF_ID")
                        .IsUnique();

                    b.ToTable("STAFF", "HOSPITAL");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Worker.Worker", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("FIRST_NAME");

                    b.Property<string>("PatrName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("PATR_NAME");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("SUR_NAME");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "IDX_WORKER_ID")
                        .IsUnique();

                    b.ToTable("WORKER", "HOSPITAL");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Client.Client", b =>
                {
                    b.HasOne("ProdoctorovIntegration.Domain.Client.ClientContact", null)
                        .WithOne("Client")
                        .HasForeignKey("ProdoctorovIntegration.Domain.Client.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProdoctorovIntegration.Domain.Event", null)
                        .WithOne("Client")
                        .HasForeignKey("ProdoctorovIntegration.Domain.Client.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Client.ContactTypeInfo", b =>
                {
                    b.HasOne("ProdoctorovIntegration.Domain.Client.ClientContact", null)
                        .WithOne("ContactInfoType")
                        .HasForeignKey("ProdoctorovIntegration.Domain.Client.ContactTypeInfo", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Event", b =>
                {
                    b.HasOne("ProdoctorovIntegration.Domain.Worker.Worker", "InsertUser")
                        .WithMany()
                        .HasForeignKey("InsertUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProdoctorovIntegration.Domain.Worker.Worker", "Worker")
                        .WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InsertUser");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Worker.Staff", b =>
                {
                    b.HasOne("ProdoctorovIntegration.Domain.Worker.Worker", null)
                        .WithOne("Staff")
                        .HasForeignKey("ProdoctorovIntegration.Domain.Worker.Staff", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Worker.Worker", b =>
                {
                    b.HasOne("ProdoctorovIntegration.Domain.Event", null)
                        .WithOne("UpdateUser")
                        .HasForeignKey("ProdoctorovIntegration.Domain.Worker.Worker", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Client.ClientContact", b =>
                {
                    b.Navigation("Client")
                        .IsRequired();

                    b.Navigation("ContactInfoType")
                        .IsRequired();
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Event", b =>
                {
                    b.Navigation("Client")
                        .IsRequired();

                    b.Navigation("UpdateUser")
                        .IsRequired();
                });

            modelBuilder.Entity("ProdoctorovIntegration.Domain.Worker.Worker", b =>
                {
                    b.Navigation("Staff")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
