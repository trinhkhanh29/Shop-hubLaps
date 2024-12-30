using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTinTuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cập nhật bảng TinTuc để các trường hinhnen, tomtat, noidung, luotxem, ngaycapnhat và xuatban có thể nullable
            migrationBuilder.AlterColumn<string>(
                name: "hinhnen",
                table: "TinTuc",
                type: "nvarchar(255)",
                nullable: true, // Cho phép null
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "tomtat",
                table: "TinTuc",
                type: "nvarchar(255)",
                nullable: true, // Cho phép null
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "noidung",
                table: "TinTuc",
                type: "ntext",
                nullable: true, // Cho phép null
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "luotxem",
                table: "TinTuc",
                type: "int",
                nullable: true, // Cho phép null
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngaycapnhat",
                table: "TinTuc",
                type: "smalldatetime",
                nullable: true, // Cho phép null
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldNullable: false);

            migrationBuilder.AlterColumn<bool>(
                name: "xuatban",
                table: "TinTuc",
                type: "bit",
                nullable: true, // Cho phép null
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Khôi phục lại các trường thành không nullable
            migrationBuilder.AlterColumn<string>(
                name: "hinhnen",
                table: "TinTuc",
                type: "nvarchar(255)",
                nullable: false, // Không cho phép null
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tomtat",
                table: "TinTuc",
                type: "nvarchar(255)",
                nullable: false, // Không cho phép null
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "noidung",
                table: "TinTuc",
                type: "ntext",
                nullable: false, // Không cho phép null
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "luotxem",
                table: "TinTuc",
                type: "int",
                nullable: false, // Không cho phép null
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngaycapnhat",
                table: "TinTuc",
                type: "smalldatetime",
                nullable: false, // Không cho phép null
                oldClrType: typeof(DateTime),
                oldType: "smalldatetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "xuatban",
                table: "TinTuc",
                type: "bit",
                nullable: false, // Không cho phép null
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
