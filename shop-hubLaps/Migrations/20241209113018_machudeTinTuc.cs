using Microsoft.EntityFrameworkCore.Migrations;

namespace shop_hubLaps.Migrations
{
    public partial class machudeTinTuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thay đổi trường machude từ nullable thành non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "machude",
                table: "TinTuc",
                nullable: false, // Không cho phép null
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Quay lại trạng thái nullable cho trường machude nếu cần
            migrationBuilder.AlterColumn<int>(
                name: "machude",
                table: "TinTuc",
                nullable: true, // Cho phép null
                oldClrType: typeof(int));
        }
    }
}
