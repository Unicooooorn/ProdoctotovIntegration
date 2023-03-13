using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdoctorovIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityTypeConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CLIENT_CLIENT_CONTACT_ID",
                schema: "HOSPITAL",
                table: "CLIENT");

            migrationBuilder.DropForeignKey(
                name: "FK_CONTACT_TYPE_CLIENT_CONTACT_ID",
                schema: "HOSPITAL",
                table: "CONTACT_TYPE");

            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_WORKER_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_WORKER_WorkerId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.DropForeignKey(
                name: "FK_STAFF_WORKER_ID",
                schema: "HOSPITAL",
                table: "STAFF");

            migrationBuilder.DropForeignKey(
                name: "FK_WORKER_EVENT_ID",
                schema: "HOSPITAL",
                table: "WORKER");

            migrationBuilder.DropIndex(
                name: "IX_EVENT_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.DropIndex(
                name: "IX_EVENT_WorkerId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.DropColumn(
                name: "InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.AddForeignKey(
                name: "FK_CLICON_CLIID",
                schema: "HOSPITAL",
                table: "CLIENT",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "CLIENT_CONTACT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CLICON_CONTYPEID",
                schema: "HOSPITAL",
                table: "CONTACT_TYPE",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "CLIENT_CONTACT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WORKER_STAFF_ID",
                schema: "HOSPITAL",
                table: "STAFF",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "WORKER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_WORKER_ID",
                schema: "HOSPITAL",
                table: "WORKER",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "EVENT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CLICON_CLIID",
                schema: "HOSPITAL",
                table: "CLIENT");

            migrationBuilder.DropForeignKey(
                name: "FK_CLICON_CONTYPEID",
                schema: "HOSPITAL",
                table: "CONTACT_TYPE");

            migrationBuilder.DropForeignKey(
                name: "FK_WORKER_STAFF_ID",
                schema: "HOSPITAL",
                table: "STAFF");

            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_WORKER_ID",
                schema: "HOSPITAL",
                table: "WORKER");

            migrationBuilder.AddColumn<Guid>(
                name: "InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerId",
                schema: "HOSPITAL",
                table: "EVENT",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "InsertUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_WorkerId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CLIENT_CLIENT_CONTACT_ID",
                schema: "HOSPITAL",
                table: "CLIENT",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "CLIENT_CONTACT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CONTACT_TYPE_CLIENT_CONTACT_ID",
                schema: "HOSPITAL",
                table: "CONTACT_TYPE",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "CLIENT_CONTACT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_WORKER_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "InsertUserId",
                principalSchema: "HOSPITAL",
                principalTable: "WORKER",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_WORKER_WorkerId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "WorkerId",
                principalSchema: "HOSPITAL",
                principalTable: "WORKER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_STAFF_WORKER_ID",
                schema: "HOSPITAL",
                table: "STAFF",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "WORKER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WORKER_EVENT_ID",
                schema: "HOSPITAL",
                table: "WORKER",
                column: "ID",
                principalSchema: "HOSPITAL",
                principalTable: "EVENT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
