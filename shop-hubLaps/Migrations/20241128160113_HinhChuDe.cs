using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class HinhChuDe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                          name: "hinh",
                          table: "ChuDe",
                          type: "nvarchar(max)",
                          nullable: true, // Allow null values
                          oldClrType: typeof(string),
                          oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                 name: "hinh",
                 table: "ChuDe",
                 type: "nvarchar(max)",
                 nullable: false, // Revert to not allowing null values
                 defaultValue: "",
                 oldClrType: typeof(string),
                 oldType: "nvarchar(max)",
                 oldNullable: true);
        }
    }
}
