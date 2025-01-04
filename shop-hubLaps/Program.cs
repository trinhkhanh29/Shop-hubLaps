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
using shop_hubLaps.Services;
using shop_hubLaps.Service;
using System.Text.Json.Serialization;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using shop_hubLaps.Models.Momo;
using shop_hubLaps.Service.Vnpay;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using shop_hubLaps.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmailService>();

// Add Login-Microsoft
builder.Services.AddAuthentication()
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        options.CallbackPath = "/signin-microsoftt";
    });

// Configure Momo Options
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

//Connect VNPay API
builder.Services.AddScoped<IVnPayService, VnPayService>();

// Cấu hình Serilog cho logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AzureSqlDbConnection")
                       ?? throw new InvalidOperationException("Connection string 'AzureSqlDbConnection' not found.");

builder.Services.AddDbContext<DBContextSample>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

// Đăng ký DbContext cho DataModel
builder.Services.AddDbContext<DataModel>(options =>
    options.UseSqlServer(connectionString));  // Dùng SQL Server cho DataModel

// Đăng ký dịch vụ LaptopService
builder.Services.AddScoped<ILaptopService, LaptopService>();
// Đăng ký CartService
builder.Services.AddScoped<ICartService, CartService>();
//// Kết nối tới database
//var connectionString = builder.Configuration.GetConnectionString("DBContextSampleConnection")
//                       ?? throw new InvalidOperationException("Connection string 'DBContextSampleConnection' not found.");

//// Add DbContext with SQL Server
//builder.Services.AddDbContext<DBContextSample>(options =>
//    options.UseSqlServer(connectionString));

//// Thêm DbContext cho DataModel
//builder.Services.AddDbContext<DataModel>(options =>
//    options.UseSqlServer(connectionString));

// Cấu hình Identity với vai trò
builder.Services.AddDefaultIdentity<SampleUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Thêm hỗ trợ cho vai trò
    .AddEntityFrameworkStores<DBContextSample>();

builder.Services.AddScoped<IUserService, UserService>();

// Cấu hình cache cho session
builder.Services.AddDistributedMemoryCache(); // Cấu hình bộ nhớ phân tán cho session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Giới hạn quyền truy cập cookie
    options.Cookie.IsEssential = true; // Đảm bảo cookie luôn có sẵn
});
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Identity/Account/Login");
});


var app = builder.Build();



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

// Thêm middleware ActionLoggingMiddleware vào pipeline
app.UseMiddleware<ActionLoggingMiddleware>();

app.UseRouting();

// Đăng ký UseAuthentication và UseAuthorization trước ActionLoggingMiddleware
app.UseAuthentication();
app.UseAuthorization();

// Bật session middleware
app.UseSession(); // Cấu hình session middleware

// Thêm middleware ActionLoggingMiddleware vào pipeline sau khi đã xác thực người dùng
app.UseMiddleware<ActionLoggingMiddleware>();

// Cấu hình route cho controller và Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

public class AppSettings
{
    public string TimeZoneId { get; set; }
}