using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    public partial class UpdateRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the Foreign Key relationship between ChiTietDonHang and DonHang
            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHangs_madon",
                table: "ChiTietDonHang",
                column: "madon",
                principalTable: "DonHang",
                principalColumn: "madon",
                onDelete: ReferentialAction.Cascade);

            // Create the Foreign Key relationship between ChiTietDonHang and Laptop
            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_Laptops_malaptop",
                table: "ChiTietDonHang",
                column: "malaptop",
                principalTable: "Laptop",
                principalColumn: "malaptop",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the Foreign Key relationship between ChiTietDonHang and DonHang
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHangs_madon",
                table: "ChiTietDonHang");

            // Drop the Foreign Key relationship between ChiTietDonHang and Laptop
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_Laptops_malaptop",
                table: "ChiTietDonHang");
        }
    }
}
