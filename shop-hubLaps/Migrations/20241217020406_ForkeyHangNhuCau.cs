using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class ForkeyHangNhuCau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add foreign key constraint for mahang (Foreign Key to Hang table)
            migrationBuilder.AddForeignKey(
                name: "FK_Laptop_Hang_mahang",
                table: "Laptop",
                column: "mahang",
                principalTable: "Hang",
                principalColumn: "mahang",
                onDelete: ReferentialAction.Restrict);

            // Add foreign key constraint for manhucau (Foreign Key to NhuCau table)
            migrationBuilder.AddForeignKey(
                name: "FK_Laptop_NhuCau_manhucau",
                table: "Laptop",
                column: "manhucau",
                principalTable: "NhuCau",
                principalColumn: "manhucau",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the foreign key for mahang
            migrationBuilder.DropForeignKey(
                name: "FK_Laptop_Hang_mahang",
                table: "Laptop");

            // Remove the foreign key for manhucau
            migrationBuilder.DropForeignKey(
                name: "FK_Laptop_NhuCau_manhucau",
                table: "Laptop");
        }
    }
}
