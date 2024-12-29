using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class tongChiTietDonHang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột tong vào bảng ChiTietDonHang
            migrationBuilder.AddColumn<decimal>(
                name: "tong",
                table: "ChiTietDonHang",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
