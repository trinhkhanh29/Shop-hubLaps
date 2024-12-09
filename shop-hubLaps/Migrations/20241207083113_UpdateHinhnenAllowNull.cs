using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

    public partial class UpdateHinhnenAllowNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Đảm bảo rằng cột hinhnen cho phép NULL
            migrationBuilder.AlterColumn<string>(
                name: "hinhnen",
                table: "TinTuc",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nếu bạn muốn quay lại trạng thái trước đó, có thể đặt lại cột này thành NOT NULL
            migrationBuilder.AlterColumn<string>(
                name: "hinhnen",
                table: "TinTuc",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);
        }
    }

}
