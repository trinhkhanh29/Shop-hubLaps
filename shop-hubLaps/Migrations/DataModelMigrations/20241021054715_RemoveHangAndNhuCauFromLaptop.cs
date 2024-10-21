using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    /// <inheritdoc />
    public partial class RemoveHangAndNhuCauFromLaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa ngoại trước khi xóa cột
            migrationBuilder.DropForeignKey(
                name: "FK_Laptop_Hang_Hangmahang",
                table: "Laptop");

            migrationBuilder.DropForeignKey(
                name: "FK_Laptop_NhuCau_NhuCaumanhucau",
                table: "Laptop");

            // Xóa các cột Hang và NhuCau
            migrationBuilder.DropColumn(
                name: "Hangmahang",
                table: "Laptop");

            migrationBuilder.DropColumn(
                name: "NhuCaumanhucau",
                table: "Laptop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Khôi phục các cột bị xóa
            migrationBuilder.AddColumn<int>(
                name: "Hangmahang",
                table: "Laptop",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NhuCaumanhucau",
                table: "Laptop",
                type: "int",
                nullable: true);

            // Thêm lại khóa ngoại
            migrationBuilder.AddForeignKey(
                name: "FK_Laptop_Hang_Hangmahang",
                table: "Laptop",
                column: "Hangmahang",
                principalTable: "Hang",
                principalColumn: "mahang");

            migrationBuilder.AddForeignKey(
                name: "FK_Laptop_NhuCau_NhuCaumanhucau",
                table: "Laptop",
                column: "NhuCaumanhucau",
                principalTable: "NhuCau",
                principalColumn: "manhucau");
        }
    }
}
