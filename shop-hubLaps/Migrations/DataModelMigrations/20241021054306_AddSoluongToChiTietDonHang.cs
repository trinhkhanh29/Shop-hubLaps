using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    /// <inheritdoc />
    public partial class AddSoluongToChiTietDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột soluong vào bảng ChiTietDonHang
            migrationBuilder.AddColumn<int>(
                name: "soluong",
                table: "ChiTietDonHang",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa cột soluong khỏi bảng ChiTietDonHang nếu cần
            migrationBuilder.DropColumn(
                name: "soluong",
                table: "ChiTietDonHang");
        }
    }
}
