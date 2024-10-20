using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shop_hubLaps.Areas.Identity.Data;

namespace shop_hubLaps.Areas.Identity.Data;

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
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<SampleUser>
    {
        public void Configure(EntityTypeBuilder<SampleUser> builder)
        {
            builder.Property(user => user.FirstName).HasMaxLength(100);

            builder.Property(user => user.LastName).HasMaxLength(100);

            builder.Property(user => user.NgaySinh).IsRequired(false);

            builder.Property(user => user.Profile).HasColumnType("nvarchar(max)"); 

            builder.Property(user => user.Avatar).HasMaxLength(70);

            builder.Property(user => user.HoTen).HasMaxLength(50);

            builder.Property(user => user.DiaChi).HasMaxLength(200);
        }
    }
}

