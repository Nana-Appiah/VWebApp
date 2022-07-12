using VerificationWebApp.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* configuration object...newly added */
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services.AddDbContext<VerificationWebApp.DbData.IDVerificationTestContext>(options =>
{
    options.UseSqlServer(settings.connection);
});

ConfigObject.KONNECT = settings.connection;
ConfigObject.API = settings.apiUrl;

/* user configuration starts here */

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

