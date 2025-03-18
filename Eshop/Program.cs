using Eshop.Business.Managers;
using Eshop.Data;
using Eshop.Data.Ineterfaces;
using Eshop.Data.Models;
using Eshop.Data.Repositories;
using Eshop.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Eshop.Business.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .UseLazyLoadingProxies()
    .ConfigureWarnings(x => x.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ICategoryManager, CategoryManager>();
builder.Services.AddScoped<IProductManager,  ProductManager>();
builder.Services.AddScoped<IImageManager, ImageManager>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}");
app.MapRazorPages();

await app.RegisterAdmin("admin@eshop.cz", "Admin123");

app.Run();
