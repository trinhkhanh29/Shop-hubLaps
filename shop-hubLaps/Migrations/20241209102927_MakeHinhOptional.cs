using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    public partial class MakeHinhOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thay đổi cột 'hinh' thành nullable
            migrationBuilder.AlterColumn<string>(
                name: "hinh",
                table: "Hang",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70,
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Đảo ngược thay đổi và yêu cầu 'hinh' không nullable
            migrationBuilder.AlterColumn<string>(
                name: "hinh",
                table: "Hang",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70,
                oldNullable: true);
        }
    }
}
