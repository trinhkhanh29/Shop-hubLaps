using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_hubLaps.Migrations.DataModelMigrations
{
    /// <inheritdoc />
    public partial class UpdateDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuDe",
                columns: table => new
                {
                    machude = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenchude = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    slug = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    hinh = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuDe", x => x.machude);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    madon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thanhtoan = table.Column<bool>(type: "bit", nullable: true),
                    giaohang = table.Column<bool>(type: "bit", nullable: true),
                    ngaydat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngaygiao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    makh = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    tinhtrang = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.madon);
                });

            migrationBuilder.CreateTable(
                name: "Hang",
                columns: table => new
                {
                    mahang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenhang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    hinh = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hang", x => x.mahang);
                });

            migrationBuilder.CreateTable(
                name: "LienHe",
                columns: table => new
                {
                    malienhe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hoten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(254)", unicode: false, maxLength: 254, nullable: false),
                    dienthoai = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    noidung = table.Column<string>(type: "ntext", nullable: false),
                    trangthai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHe", x => x.malienhe);
                });

            migrationBuilder.CreateTable(
                name: "NhuCau",
                columns: table => new
                {
                    manhucau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tennhucau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhuCau", x => x.manhucau);
                });

            migrationBuilder.CreateTable(
                name: "QuangCao",
                columns: table => new
                {
                    maqc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenqc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tencongty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    hinhnen = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    link = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ngaybatdau = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ngayhethan = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    trangthai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuangCao", x => x.maqc);
                });

            migrationBuilder.CreateTable(
                name: "TinTuc",
                columns: table => new
                {
                    matin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tieude = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    hinhnen = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    tomtat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    noidung = table.Column<string>(type: "ntext", nullable: false),
                    luotxem = table.Column<int>(type: "int", nullable: true),
                    ngaycapnhat = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    xuatban = table.Column<bool>(type: "bit", nullable: true),
                    machude = table.Column<int>(type: "int", nullable: true),
                    ChuDemachude = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinTuc", x => x.matin);
                    table.ForeignKey(
                        name: "FK_TinTuc_ChuDe_ChuDemachude",
                        column: x => x.ChuDemachude,
                        principalTable: "ChuDe",
                        principalColumn: "machude",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Laptop",
                columns: table => new
                {
                    malaptop = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenlaptop = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    giaban = table.Column<decimal>(type: "decimal(18,0)", precision: 18, scale: 0, nullable: true),
                    mota = table.Column<string>(type: "ntext", nullable: false),
                    hinh = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    mahang = table.Column<int>(type: "int", nullable: true),
                    manhucau = table.Column<int>(type: "int", nullable: true),
                    cpu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gpu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ram = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    hardware = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    manhinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ngaycapnhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    soluongton = table.Column<int>(type: "int", nullable: true),
                    pin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    trangthai = table.Column<bool>(type: "bit", nullable: true),
                    Hangmahang = table.Column<int>(type: "int", nullable: false),
                    NhuCaumanhucau = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptop", x => x.malaptop);
                    table.ForeignKey(
                        name: "FK_Laptop_Hang_Hangmahang",
                        column: x => x.Hangmahang,
                        principalTable: "Hang",
                        principalColumn: "mahang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Laptop_NhuCau_NhuCaumanhucau",
                        column: x => x.NhuCaumanhucau,
                        principalTable: "NhuCau",
                        principalColumn: "manhucau",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinhLuan",
                columns: table => new
                {
                    mabinhluan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    noidung = table.Column<string>(type: "ntext", nullable: false),
                    vote = table.Column<int>(type: "int", nullable: true),
                    ngaybinhluan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    matin = table.Column<int>(type: "int", nullable: true),
                    trangthai = table.Column<bool>(type: "bit", nullable: true),
                    TinTucmatin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuan", x => x.mabinhluan);
                    table.ForeignKey(
                        name: "FK_BinhLuan_TinTuc_TinTucmatin",
                        column: x => x.TinTucmatin,
                        principalTable: "TinTuc",
                        principalColumn: "matin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    madanhgia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    noidung = table.Column<string>(type: "ntext", nullable: false),
                    vote = table.Column<int>(type: "int", nullable: true),
                    ngaydanhgia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    malaptop = table.Column<int>(type: "int", nullable: true),
                    trangthai = table.Column<bool>(type: "bit", nullable: true),
                    Laptopmalaptop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.madanhgia);
                    table.ForeignKey(
                        name: "FK_DanhGia_Laptop_Laptopmalaptop",
                        column: x => x.Laptopmalaptop,
                        principalTable: "Laptop",
                        principalColumn: "malaptop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetaLaptop",
                columns: table => new
                {
                    mameta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    keymeta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    valuemeta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    malaptop = table.Column<int>(type: "int", nullable: true),
                    Laptopmalaptop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaLaptop", x => x.mameta);
                    table.ForeignKey(
                        name: "FK_MetaLaptop_Laptop_Laptopmalaptop",
                        column: x => x.Laptopmalaptop,
                        principalTable: "Laptop",
                        principalColumn: "malaptop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_TinTucmatin",
                table: "BinhLuan",
                column: "TinTucmatin");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_Laptopmalaptop",
                table: "DanhGia",
                column: "Laptopmalaptop");

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_Hangmahang",
                table: "Laptop",
                column: "Hangmahang");

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_NhuCaumanhucau",
                table: "Laptop",
                column: "NhuCaumanhucau");

            migrationBuilder.CreateIndex(
                name: "IX_MetaLaptop_Laptopmalaptop",
                table: "MetaLaptop",
                column: "Laptopmalaptop");

            migrationBuilder.CreateIndex(
                name: "IX_TinTuc_ChuDemachude",
                table: "TinTuc",
                column: "ChuDemachude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinhLuan");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "LienHe");

            migrationBuilder.DropTable(
                name: "MetaLaptop");

            migrationBuilder.DropTable(
                name: "QuangCao");

            migrationBuilder.DropTable(
                name: "TinTuc");

            migrationBuilder.DropTable(
                name: "Laptop");

            migrationBuilder.DropTable(
                name: "ChuDe");

            migrationBuilder.DropTable(
                name: "Hang");

            migrationBuilder.DropTable(
                name: "NhuCau");
        }
    }
}
