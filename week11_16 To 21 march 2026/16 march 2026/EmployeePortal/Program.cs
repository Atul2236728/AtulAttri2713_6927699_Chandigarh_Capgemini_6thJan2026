using EmployeePortal.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});

builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddSession();

var app = builder.Build();

// Middleware
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");