using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeCreateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the CreatedDate column to the VnpayModel table
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "VnpayModels",
                type: "datetime2", // Specify the type based on your database (datetime2 for SQL Server)
                nullable: false,
                defaultValue: DateTime.Now); // Set default value to the current time
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the CreatedDate column in case of rollback
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "VnpayModels");
        }
    }
}
