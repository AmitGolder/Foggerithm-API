using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foggerithm.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Devices");
        }
    }
}
