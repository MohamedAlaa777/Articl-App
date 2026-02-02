using ArticlApp.Core;
using ArticlApp.Data;
using ArticlApp.Data.Interfaces;
using ArticlApp.Data.SqlServerEF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= SERVICES =================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", p => p.RequireClaim("User", "User"));
    options.AddPolicy("Admin", p => p.RequireClaim("Admin", "Admin"));
});

// Data helpers (SCOPED ✔)
builder.Services.AddScoped<IDataHelper<Category>, CategoryEntity>();
builder.Services.AddScoped<IDataHelper<Author>, AuthorEntity>();
builder.Services.AddScoped<IDataByUserHelper<AuthorPost>, AuthorPostEntity>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ================= APP =================

var app = builder.Build();

// ================= PIPELINE =================

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
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

app.MapRazorPages();

// 🔴 THIS WAS MISSING
app.Run();
