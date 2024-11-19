using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations
{
    public partial class AddProfilePicturePathToSampleUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the new ProfilePicturePath column to AspNetUsers table
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                type: "nvarchar(200)", // Adjust the type and length as needed
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the ProfilePicturePath column from AspNetUsers table
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers");
        }
    }
}
