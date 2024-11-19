using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shop_hubLaps.Models;
using shop_hubLaps.Middleware; 
using Serilog; 

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Serilog cho logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

// Kết nối tới database
var connectionString = builder.Configuration.GetConnectionString("DBContextSampleConnection")
                       ?? throw new InvalidOperationException("Connection string 'DBContextSampleConnection' not found.");

// Add DbContext with SQL Server
builder.Services.AddDbContext<DBContextSample>(options =>
    options.UseSqlServer(connectionString));

// Thêm DbContext cho DataModel
builder.Services.AddDbContext<DataModel>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Identity với vai trò
builder.Services.AddDefaultIdentity<SampleUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Thêm hỗ trợ cho vai trò
    .AddEntityFrameworkStores<DBContextSample>();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Thêm middleware ActionLoggingMiddleware vào pipeline
app.UseMiddleware<ActionLoggingMiddleware>();

// Cấu hình pipeline xử lý HTTP requests
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware bảo mật và xử lý yêu cầu tĩnh
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Đăng ký UseAuthentication và UseAuthorization trước ActionLoggingMiddleware
app.UseAuthentication();
app.UseAuthorization();

// Thêm middleware ActionLoggingMiddleware vào pipeline sau khi đã xác thực người dùng
app.UseMiddleware<ActionLoggingMiddleware>();

// Cấu hình route cho controller và Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
