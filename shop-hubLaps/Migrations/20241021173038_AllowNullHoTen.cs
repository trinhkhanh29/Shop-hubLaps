using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullHoTen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "AspNetUsers",
                nullable: true, // Cho phép giá trị null
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"); // Thay đổi kiểu dữ liệu nếu cần
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false, // Trả lại trạng thái không cho phép giá trị null
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
