using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    public partial class UpdateDatabaseSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa mối quan hệ và chỉ mục cũ
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_SampleUsers_UserId",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "soluong",
                table: "ChiTietDonHang");

            // Cập nhật các trường trong bảng SampleUsers
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "SampleUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "SampleUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "SampleUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "SampleUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "SampleUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "SampleUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "SampleUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "SampleUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "SampleUsers",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false);

            // Cập nhật bảng ChiTietDonHang
            migrationBuilder.AlterColumn<decimal>(
                name: "dongia",
                table: "ChiTietDonHang",
                type: "decimal(18,0)",
                precision: 18,
                scale: 0,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "ChiTietDonHang",
                type: "int",
                nullable: false)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "madon",
                table: "ChiTietDonHang",
                type: "int",
                nullable: false)
                .Annotation("Relational:ColumnOrder", 0);

            // Tạo lại bảng DonHang với mối quan hệ đúng
            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    // Thêm các cột khác của DonHang tại đây
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonHang_SampleUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SampleUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Thêm chỉ mục cho UserId
            migrationBuilder.CreateIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Gỡ bỏ bảng DonHang
            migrationBuilder.DropTable(name: "DonHang");

            // Quay lại các thay đổi trước đó
            migrationBuilder.DropIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedUserName",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedEmail",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "HoTen",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "SampleUsers",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "dongia",
                table: "ChiTietDonHang",
                type: "decimal(18,0)",
                precision: 18,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "malaptop",
                table: "ChiTietDonHang",
                type: "int",
                nullable: false)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "madon",
                table: "ChiTietDonHang",
                type: "int",
                nullable: false)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "soluong",
                table: "ChiTietDonHang",
                type: "int",
                nullable: true);
        }
    }
}
