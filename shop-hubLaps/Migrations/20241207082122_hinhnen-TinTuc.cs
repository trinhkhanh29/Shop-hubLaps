using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class hinhnenTinTuc : Migration
    {
     
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.AlterColumn<string>(
                    name: "hinhnen",
                    table: "TinTuc",
                    maxLength: 255,  // Cập nhật chiều dài mới của trường
                    nullable: true,  // Nếu trường có thể null
                    oldClrType: typeof(string),
                    oldMaxLength: 70);
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.AlterColumn<string>(
                    name: "hinhnen",
                    table: "TinTuc",
                    maxLength: 70,  // Cập nhật lại chiều dài cũ nếu cần rollback
                    nullable: true,
                    oldClrType: typeof(string),
                    oldMaxLength: 255);
            }
        

    }
}
