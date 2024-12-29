using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class AddGiaToDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột 'gia' vào bảng ChiTietDonHang
            migrationBuilder.AddColumn<decimal>(
                name: "gia",
                table: "ChiTietDonHang",
                type: "decimal(18,2)", // Định dạng số thập phân với 2 chữ số sau dấu
                nullable: false,
                defaultValue: 0m); // Giá trị mặc định là 0
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa cột 'gia' nếu migration bị rollback
            migrationBuilder.DropColumn(
                name: "gia",
                table: "ChiTietDonHang");
        }
    }
}
