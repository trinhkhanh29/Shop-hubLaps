using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class GiaToDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột 'gia' vào bảng 'DonHang'
            migrationBuilder.AddColumn<decimal>(
                name: "gia",
                table: "DonHang",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            // Cập nhật giá trị cho cột 'gia' trong bảng 'DonHang'
            migrationBuilder.Sql(@"
                UPDATE DonHang
                SET gia = (
                    SELECT SUM(CT.dongia * CT.soluong)
                    FROM ChiTietDonHang CT
                    WHERE CT.madon = DonHang.madon
                )
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
