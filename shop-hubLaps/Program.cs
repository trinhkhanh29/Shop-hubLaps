using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using shop_hubLaps.Models;

var builder = WebApplication.CreateBuilder(args);

// Connection string for the database
var connectionString = builder.Configuration.GetConnectionString("DBContextSampleConnection")
                       ?? throw new InvalidOperationException("Connection string 'DBContextSampleConnection' not found.");

// Add DbContext with SQL Server
builder.Services.AddDbContext<DBContextSample>(options =>
    options.UseSqlServer(connectionString));

// Thêm DbContext cho DataModel
builder.Services.AddDbContext<DataModel>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity with default settings
builder.Services.AddDefaultIdentity<SampleUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Require email confirmation
})
.AddEntityFrameworkStores<DBContextSample>(); // Kết nối với DbContext

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Handle errors in production
    app.UseHsts(); // Enable HSTS
}

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

// Đăng ký UseAuthentication và UseAuthorization trước
app.UseAuthentication();
app.UseAuthorization();

// Định nghĩa route mặc định cho controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Định nghĩa route cho Razor Pages
app.MapRazorPages();

// Chạy ứng dụng
app.Run();
