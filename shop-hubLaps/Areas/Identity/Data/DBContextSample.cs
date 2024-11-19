using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;

namespace shop_hubLaps.Areas.Identity.Data
{
    public class DBContextSample : IdentityDbContext<SampleUser>
    {
        public DBContextSample(DbContextOptions<DBContextSample> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

            SeedRoles(builder);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            // Danh sách các vai trò cần được thêm vào
            var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Name = "User", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new IdentityRole { Name = "Staff", NormalizedName = "STAFF", ConcurrencyStamp = Guid.NewGuid().ToString() }
        };

            // Kiểm tra và thêm vai trò
            foreach (var role in roles)
            {
                builder.Entity<IdentityRole>().HasData(role);
            }

        }
        // Cấu hình thực thể SampleUser
        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<SampleUser>
        {
            public void Configure(EntityTypeBuilder<SampleUser> builder)
            {
                // Cấu hình các thuộc tính của SampleUser
                builder.Property(user => user.FirstName).HasMaxLength(100);
                builder.Property(user => user.LastName).HasMaxLength(100);
                builder.Property(user => user.NgaySinh).IsRequired(false);
                builder.Property(user => user.Profile).HasColumnType("nvarchar(max)");
                builder.Property(user => user.Avatar).HasMaxLength(70);
                builder.Property(user => user.DiaChi).HasMaxLength(200);
            }
        }
    }
}
