using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    public partial class FixForeignKeyConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing foreign key constraint if it exists
            migrationBuilder.DropForeignKey(
                name: "FK_MetaLaptop_Laptop_Laptopmalaptop",
                table: "MetaLaptop");

            // Cập nhật khóa ngoại malaptop để không cho phép NULL
            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "MetaLaptop",
                nullable: false,  // Đảm bảo không cho phép NULL
                oldClrType: typeof(int),
                oldNullable: true);

            // Tạo lại liên kết khóa ngoại với bảng Laptop
            migrationBuilder.AddForeignKey(
                name: "FK_MetaLaptop_Laptop_Laptopmalaptop",
                table: "MetaLaptop",
                column: "malaptop",
                principalTable: "Laptop",
                principalColumn: "malaptop",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Đảo ngược việc thay đổi cột malaptop về cho phép NULL
            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "MetaLaptop",
                nullable: true, // Để cho phép NULL
                oldClrType: typeof(int),
                oldNullable: false);

            // Tạo lại khóa ngoại cũ nếu cần
            migrationBuilder.AddForeignKey(
                name: "FK_MetaLaptop_Laptop_Laptopmalaptop",
                table: "MetaLaptop",
                column: "malaptop",
                principalTable: "Laptop",
                principalColumn: "malaptop",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
