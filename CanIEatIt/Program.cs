using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CanIEatIt.Data;
using CanIEatIt.Models;
using CanIEatIt.Services;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CanIEatItContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CanIEatItContext") ?? throw new InvalidOperationException("Connection string 'CanIEatItContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CanIEatItContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication("DefaultScheme")
    .AddCookie("DefaultScheme", options =>
    {
        options.Cookie.Name = "AppCookie";
        options.LoginPath = "/";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NormalUser", policy =>
    {
        policy.RequireRole("BasicUser");
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
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

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "BasicUser"),
            new Claim(ClaimTypes.Role, "BasicUser")
        }, "DefaultScheme");

		var principle = new ClaimsPrincipal(identity);

        await context.SignInAsync("DefaultScheme", principle);
	}
	await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
