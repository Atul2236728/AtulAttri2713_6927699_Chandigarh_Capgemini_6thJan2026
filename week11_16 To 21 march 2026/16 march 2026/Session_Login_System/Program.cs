using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Enable Session
builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use Session
app.UseSession();

app.UseRouting();
app.UseAuthorization();

// Default route → Login page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();