using blog2.Data;
using blog2.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<BlogAppDbContext>();

// builder.Services.AddAuthentication("CookieAuthenticationScheme")
//     .AddCookie("CookieAuthenticationScheme", options =>
//     {
//         options.Cookie.Name = "BlogApp.Identity";
//         options.Cookie.HttpOnly = false;
//         options.Cookie.Expiration = TimeSpan.FromSeconds(10);
//         options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
//         // options.AccessDeniedPath = new PathString("");
//         options.LoginPath = "account/login";
//     });

builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<SeedDatabase>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
