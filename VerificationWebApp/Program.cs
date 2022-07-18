using VerificationWebApp.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* configuration object...newly added */
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

ConfigObject.KONNECT = settings.connection;
ConfigObject.Db_API = settings.apiUrl;
ConfigObject.NIA_API = settings.imsghAPI;

ConfigObject.GhanaCardVerificationAPI = settings.verifyGHCardAPI;
ConfigObject.postCustomerDataAPI = settings.postDataAPI;

ConfigObject.API_KEY = settings.apiKey;

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

