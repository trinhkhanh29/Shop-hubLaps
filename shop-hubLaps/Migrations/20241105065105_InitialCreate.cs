using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b5325cd-b593-4781-b006-d665072a72fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13ebb5f2-8346-420d-bedb-99cfdd850945");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c064376-316a-4fe7-b97d-bd9021de9e5f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32611533-faff-4837-8528-a118e00dcc2e", "f5524472-994c-40e1-bd73-db2655af2857", "User", "USER" },
                    { "4b57cf41-6539-451c-8cfc-31ef98c6827f", "7bee936a-8e90-40dc-b148-ded8032d495c", "Admin", "ADMIN" },
                    { "bc4088a7-9038-4d24-b405-690588719d0d", "bec327ec-1e74-4612-9ba4-c38f09db4984", "Staff", "STAFF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32611533-faff-4837-8528-a118e00dcc2e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b57cf41-6539-451c-8cfc-31ef98c6827f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc4088a7-9038-4d24-b405-690588719d0d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b5325cd-b593-4781-b006-d665072a72fd", "9aa5bca4-3570-4873-ae35-a12c2f488fee", "Admin", "ADMIN" },
                    { "13ebb5f2-8346-420d-bedb-99cfdd850945", "c1255f62-0e85-4eca-8a2f-e2a848089999", "User", "USER" },
                    { "2c064376-316a-4fe7-b97d-bd9021de9e5f", "92a3fe73-0c74-4d6f-a6f4-c3c4ae223d51", "Staff", "STAFF" }
                });
        }
    }
}
