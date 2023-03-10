using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdoctorovIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContactTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_WORKER_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.AlterColumn<long>(
                name: "InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_WORKER_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "InsertUserId",
                principalSchema: "HOSPITAL",
                principalTable: "WORKER",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EVENT_WORKER_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT");

            migrationBuilder.AlterColumn<long>(
                name: "InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EVENT_WORKER_InsertUserId",
                schema: "HOSPITAL",
                table: "EVENT",
                column: "InsertUserId",
                principalSchema: "HOSPITAL",
                principalTable: "WORKER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
