using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class malaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Đảm bảo rằng không có giá trị NULL trong cột 'malaptop' trước khi thay đổi
            migrationBuilder.Sql("UPDATE MetaLaptop SET malaptop = 1 WHERE malaptop IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "MetaLaptop",
                nullable: false,  // Đảm bảo cột không nhận giá trị NULL
                oldClrType: typeof(int),
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
              name: "malaptop",
              table: "MetaLaptop",
              nullable: true,  // Trở lại nullable nếu rollback migration
              oldClrType: typeof(int),
              oldNullable: false);
        }
    }
}
