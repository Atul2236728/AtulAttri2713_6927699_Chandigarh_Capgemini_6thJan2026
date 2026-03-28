using StudentManagementSystem.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Register filters
builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();