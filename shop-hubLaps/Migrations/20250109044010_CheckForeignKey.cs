using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class CheckForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
             name: "FK_DanhGia_Laptop",
             table: "DanhGia",
             column: "malaptop",
             principalTable: "Laptop",
             principalColumn: "malaptop",
             onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        
        }
    }
}
