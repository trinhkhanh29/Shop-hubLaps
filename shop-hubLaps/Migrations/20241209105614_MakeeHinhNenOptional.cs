using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class MakeeHinhNenOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thay đổi trường hinhnen thành nullable (tùy chọn)
            migrationBuilder.AlterColumn<string>(
                name: "hinhnen",
                table: "TinTuc",
                type: "nvarchar(255)",
                nullable: true, // Cho phép giá trị NULL
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Quay lại trạng thái ban đầu, không cho phép NULL cho hinhnen
            migrationBuilder.AlterColumn<string>(
                name: "hinhnen",
                table: "TinTuc",
                type: "nvarchar(255)",
                nullable: false, // Không cho phép NULL
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: true);
        }
    }
}
