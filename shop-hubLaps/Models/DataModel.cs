using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;

namespace shop_hubLaps.Models
{
    public class DataModel : DBContextSample
    {
        public DataModel(DbContextOptions<DBContextSample> options) : base(options)
        {
        }

        public DbSet<SampleUser> SampleUsers { get; set; }
        // Khai báo các DbSet cho các bảng trong cơ sở dữ liệu
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<ChuDe> ChuDes { get; set; }
        public virtual DbSet<DanhGia> DanhGias { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<Hang> Hangs { get; set; }
        public virtual DbSet<Laptop> Laptops { get; set; }
        public virtual DbSet<LienHe> LienHes { get; set; }
        public virtual DbSet<MetaLaptop> MetaLaptops { get; set; }
        public virtual DbSet<NhuCau> NhuCaus { get; set; }
        public virtual DbSet<QuangCao> QuangCaos { get; set; }
        public virtual DbSet<TinTuc> TinTucs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DonHang>()
                .Property(d => d.makh)
                .HasColumnType("nvarchar(450)");

            modelBuilder.Entity<DonHang>()
                .HasKey(d => d.madon);

            // Cấu hình khóa chính composite cho ChiTietDonHang
            modelBuilder.Entity<ChiTietDonHang>()
                .HasKey(c => new { c.madon, c.malaptop });

            // Cấu hình mối quan hệ giữa ChiTietDonHang và DonHang
            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(e => e.DonHang)
                .WithMany(e => e.ChiTietDonHangs)
                .HasForeignKey(e => e.madon)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa ChiTietDonHang và Laptop
            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(e => e.Laptop)
                .WithMany(e => e.ChiTietDonHangs)
                .HasForeignKey(e => e.malaptop)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình các thuộc tính không dùng Unicode
            modelBuilder.Entity<ChuDe>()
                .Property(e => e.slug)
                .IsUnicode(false);

            modelBuilder.Entity<ChuDe>()
                .Property(e => e.hinh)
                .IsUnicode(false);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.tinhtrang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Hang>()
                .Property(e => e.hinh)
                .IsUnicode(false);

            modelBuilder.Entity<Laptop>()
                .Property(e => e.giaban)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Laptop>()
                .Property(e => e.hinh)
                .IsUnicode(false);

            modelBuilder.Entity<LienHe>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<LienHe>()
                .Property(e => e.dienthoai)
                .IsUnicode(false);

            modelBuilder.Entity<QuangCao>()
                .Property(e => e.hinhnen)
                .IsUnicode(false);

            modelBuilder.Entity<QuangCao>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<TinTuc>()
                .Property(e => e.hinhnen)
                .IsUnicode(false);

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.dongia)
                .HasPrecision(18, 0);
        }
    }
}
