using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    public partial class hinhLaptop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cập nhật cột hinh trong bảng Laptop để cho phép giá trị null (nullable)
            migrationBuilder.AlterColumn<string>(
                name: "hinh",
                table: "Laptop",
                type: "nvarchar(70)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldNullable: false);

            // Các thay đổi dữ liệu khác (nếu có) sẽ nằm ở đây
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Đảo lại thay đổi trong phương thức Up
            migrationBuilder.AlterColumn<string>(
                name: "hinh",
                table: "Laptop",
                type: "nvarchar(70)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldNullable: true);

            // Các thay đổi dữ liệu khác khi rollback (nếu có) sẽ nằm ở đây
        }
    }
}
