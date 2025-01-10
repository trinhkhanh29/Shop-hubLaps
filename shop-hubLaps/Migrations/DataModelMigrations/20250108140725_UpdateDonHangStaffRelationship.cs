using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    /// <inheritdoc />
    public partial class UpdateDonHangStaffRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cập nhật mối quan hệ giữa DonHang và SampleUser
            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_AspNetUsers_manv", // Tên khóa ngoại
                table: "DonHangs", // Tên bảng DonHangs
                column: "manv", // Cột chứa khóa ngoại trong DonHangs
                principalTable: "AspNetUsers", // Tên bảng của SampleUser
                principalColumn: "Id", // Cột khóa chính của bảng AspNetUsers
                onDelete: ReferentialAction.Restrict); // Không xóa dữ liệu trong DonHangs khi xóa SampleUser
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa ngoại khi rollback migration
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_AspNetUsers_manv",
                table: "DonHangs");
        }
    }
}
