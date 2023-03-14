using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdoctorovIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HOSPITAL");

            migrationBuilder.CreateTable(
                name: "CLIENT",
                schema: "HOSPITAL",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    LAST_NAME = table.Column<string>(type: "text", nullable: false),
                    FIRST_NAME = table.Column<string>(type: "text", nullable: false),
                    PATR_NAME = table.Column<string>(type: "text", nullable: false),
                    BIRTHDAY = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CLIENT_CONTACT",
                schema: "HOSPITAL",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: true),
                    CONTACT_ONLY_DIGITS = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENT_CONTACT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CLIENT_CONTACT_CLIENT_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "HOSPITAL",
                        principalTable: "CLIENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EVENT",
                schema: "HOSPITAL",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DURATION = table.Column<long>(type: "bigint", nullable: false),
                    ROOM_ID = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: true),
                    CLIENT_DATA = table.Column<string>(type: "text", nullable: false),
                    NOTE = table.Column<string>(type: "text", nullable: false),
                    IS_FOR_PRODOCTOROV = table.Column<bool>(type: "boolean", nullable: false),
                    CLAIM_ID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EVENT_CLIENT_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "HOSPITAL",
                        principalTable: "CLIENT",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WORKER",
                schema: "HOSPITAL",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    FIRST_NAME = table.Column<string>(type: "text", nullable: false),
                    PATR_NAME = table.Column<string>(type: "text", nullable: false),
                    LAST_NAME = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EVENT_WORKER_ID",
                        column: x => x.ID,
                        principalSchema: "HOSPITAL",
                        principalTable: "EVENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "STAFF",
                schema: "HOSPITAL",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    DEPARTMENT = table.Column<string>(type: "text", nullable: false),
                    SPECILITY = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAFF", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WORKER_STAFF_ID",
                        column: x => x.ID,
                        principalSchema: "HOSPITAL",
                        principalTable: "WORKER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CLIENT_ID",
                schema: "HOSPITAL",
                table: "CLIENT",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_CLIENT_CONTACT_ID",
                schema: "HOSPITAL",
                table: "CLIENT_CONTACT",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CLIENT_CONTACT_ClientId",
                schema: "HOSPITAL",
                table: "CLIENT_CONTACT",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IDX_EVENT_CLAIM_ID",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "CLAIM_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_EVENT_ID",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_ClientId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IDX_STAFF_ID",
                schema: "HOSPITAL",
                table: "STAFF",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_WORKER_ID",
                schema: "HOSPITAL",
                table: "WORKER",
                column: "ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLIENT_CONTACT",
                schema: "HOSPITAL");

            migrationBuilder.DropTable(
                name: "STAFF",
                schema: "HOSPITAL");

            migrationBuilder.DropTable(
                name: "WORKER",
                schema: "HOSPITAL");

            migrationBuilder.DropTable(
                name: "EVENT",
                schema: "HOSPITAL");

            migrationBuilder.DropTable(
                name: "CLIENT",
                schema: "HOSPITAL");
        }
    }
}
