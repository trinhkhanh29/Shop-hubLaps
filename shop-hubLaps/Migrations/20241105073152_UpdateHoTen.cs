using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop_hubLaps.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHoTen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61bb4a1f-da79-4697-8d03-09ab037b46bd", "0f2d1c1a-1ba4-4b4e-bd30-b5989683678e", "Staff", "STAFF" },
                    { "a19e1c7a-1456-4b42-a89a-69c5d389022b", "57c0b669-f878-4963-b5e7-842897bf6087", "User", "USER" },
                    { "ac9de4c4-cd8f-458a-8ddf-796efcbf65ed", "e60e1699-b419-4a4d-af9c-bccbbfce29ac", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61bb4a1f-da79-4697-8d03-09ab037b46bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a19e1c7a-1456-4b42-a89a-69c5d389022b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac9de4c4-cd8f-458a-8ddf-796efcbf65ed");

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

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
    }
}
