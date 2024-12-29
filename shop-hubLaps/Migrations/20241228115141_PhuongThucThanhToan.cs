using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class PhuongThucThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the new column 'PhuongThucThanhToan' to the 'DonHang' table
            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "DonHang",
                type: "nvarchar(50)",  // Specify the column type and length
                maxLength: 50,         // Limit length as per your needs
                nullable: true);       // The column can be nullable initially
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
