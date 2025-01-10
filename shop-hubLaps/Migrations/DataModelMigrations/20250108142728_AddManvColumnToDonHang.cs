using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    /// <inheritdoc />
    public partial class AddManvColumnToDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "manv",
                table: "DonHang",
                type: "nvarchar(450)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "manv",
                table: "DonHang");
        }
    }
}
