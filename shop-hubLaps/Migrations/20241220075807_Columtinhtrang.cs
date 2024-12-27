using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class Columtinhtrang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tinhtrang",
                table: "DonHang",
                maxLength: 10,  // Thay đổi độ dài tối đa
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5);  // Độ dài cũ

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Quay lại độ dài cũ nếu cần
            migrationBuilder.AlterColumn<string>(
                name: "tinhtrang",
                table: "DonHang",
                maxLength: 5,  // Độ dài cũ
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);  // Độ dài mới

        }
    }
}
