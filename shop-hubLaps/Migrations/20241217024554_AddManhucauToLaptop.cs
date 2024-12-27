using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class AddManhucauToLaptop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cập nhật cột 'manhucau' để cho phép giá trị null
            migrationBuilder.AlterColumn<int>(
                name: "manhucau",
                table: "Laptop",
                nullable: true,  // Cho phép giá trị null
                oldClrType: typeof(int),
                oldNullable: false);  // Cập nhật từ non-nullable sang nullable
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Quay lại cột 'manhucau' không cho phép null
            migrationBuilder.AlterColumn<int>(
                name: "manhucau",
                table: "Laptop",
                nullable: false,  // Không cho phép giá trị null
                oldClrType: typeof(int),
                oldNullable: true);  // Cập nhật từ nullable sang non-nullable
        }
    
    }
}
