using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdoctorovIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SUR_NAME",
                schema: "HOSPITAL",
                table: "WORKER",
                newName: "LAST_NAME");

            migrationBuilder.RenameColumn(
                name: "SUR_NAME",
                schema: "HOSPITAL",
                table: "CLIENT",
                newName: "LAST_NAME");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LAST_NAME",
                schema: "HOSPITAL",
                table: "WORKER",
                newName: "SUR_NAME");

            migrationBuilder.RenameColumn(
                name: "LAST_NAME",
                schema: "HOSPITAL",
                table: "CLIENT",
                newName: "SUR_NAME");
        }
    }
}
