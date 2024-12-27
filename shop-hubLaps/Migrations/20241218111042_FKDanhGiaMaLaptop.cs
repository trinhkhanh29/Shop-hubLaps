using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_hubLaps.Migrations
{
    public partial class FKDanhGiaMaLaptop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Đảm bảo cột malaptop không còn nullable
            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "DanhGia",
                type: "int",
                nullable: false, // Không cho phép null
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Thêm khóa ngoại giữa DanhGia và Laptop
            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_Laptop",
                table: "DanhGia",
                column: "malaptop",
                principalTable: "Laptop",
                principalColumn: "malaptop",
                onDelete: ReferentialAction.Restrict); // Bạn có thể thay bằng Cascade nếu muốn
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa ngoại
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_Laptop",
                table: "DanhGia");

            // Khôi phục trạng thái nullable cho cột malaptop
            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "DanhGia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: false);
        }
    }
}
