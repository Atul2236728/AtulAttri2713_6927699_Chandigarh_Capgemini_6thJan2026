using Microsoft.EntityFrameworkCore;
using EventBooking.API.Data;
using EventBooking.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ IMPORTANT: Enable IIS + Kestrel properly
builder.WebHost.UseIIS();

// Add services
builder.Services.AddControllers();

// DB Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("super_secret_key_12345678901234567890"))
    };
});

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5202","http://localhost:5200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EventBooking API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ✅ IMPORTANT: Exception page (to see real error instead of 404)
app.UseDeveloperExceptionPage();

// Middleware
app.UseCors("AllowFrontend");

// ✅ Swagger (correct way)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventBooking API V1");
    c.RoutePrefix = "swagger"; // access via /swagger
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// ✅ DB Migration + Seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();

        // Seed Events
        if (!db.Events.Any())
        {
            db.Events.AddRange(
                new Event
                {
                    Title = "Music Concert",
                    Description = "Live concert",
                    Date = DateTime.Now.AddDays(5),
                    Location = "Delhi",
                    AvailableSeats = 50
                },
                new Event
                {
                    Title = "Tech Workshop",
                    Description = "Coding session",
                    Date = DateTime.Now.AddDays(10),
                    Location = "Mumbai",
                    AvailableSeats = 30
                }
            );
        }

        // Seed Admin
        if (!db.Users.Any(u => u.Role == "admin"))
        {
            db.Users.Add(new User
            {
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("1234"),
                Role = "admin"
            });
        }

        db.SaveChanges();
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR: " + ex.Message);
    }
}

app.Run();