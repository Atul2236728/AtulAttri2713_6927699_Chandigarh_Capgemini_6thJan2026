var builder = WebApplication.CreateBuilder(args);

// ✅ Register services
builder.Services.AddControllersWithViews();

// ✅ Register HttpClient (BEST PRACTICE)
builder.Services.AddHttpClient();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();