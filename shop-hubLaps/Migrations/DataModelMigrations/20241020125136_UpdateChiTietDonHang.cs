using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    /// <inheritdoc />
    public partial class UpdateChiTietDonHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "makh",
                table: "DonHang");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DonHang",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    madon = table.Column<int>(type: "int", nullable: false),
                    malaptop = table.Column<int>(type: "int", nullable: false),
                    soluong = table.Column<int>(type: "int", nullable: true),
                    dongia = table.Column<decimal>(type: "decimal(18,0)", precision: 18, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => new { x.madon, x.malaptop });
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_madon",
                        column: x => x.madon,
                        principalTable: "DonHang",
                        principalColumn: "madon",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_Laptop_malaptop",
                        column: x => x.malaptop,
                        principalTable: "Laptop",
                        principalColumn: "malaptop",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_malaptop",
                table: "ChiTietDonHang",
                column: "malaptop");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_IdentityUser_UserId",
                table: "DonHang",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_IdentityUser_UserId",
                table: "DonHang");

            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DonHang");

            migrationBuilder.AddColumn<string>(
                name: "makh",
                table: "DonHang",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
