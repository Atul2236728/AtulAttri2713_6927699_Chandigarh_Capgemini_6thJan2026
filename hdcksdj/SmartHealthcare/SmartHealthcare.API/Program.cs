using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SmartHealthcare.API.Data;
using SmartHealthcare.API.Helpers;
using SmartHealthcare.API.Mappings;
using SmartHealthcare.API.Middleware;
using SmartHealthcare.API.Repositories;
using SmartHealthcare.API.Repositories.Interfaces;
using SmartHealthcare.API.Services;
using SmartHealthcare.API.Services.Interfaces;
using System.Text;

// ── Serilog Setup ─────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// ── Use Serilog ───────────────────────────────────────
builder.Host.UseSerilog();

// ── Database ──────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Controllers ───────────────────────────────────────
builder.Services.AddControllers();

// ── AutoMapper ────────────────────────────────────────
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ── Repositories ──────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// ── Services ──────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// ── JWT Helper ────────────────────────────────────────
builder.Services.AddSingleton<JwtHelper>();

// ── Cache Helper ───────────────────────────────────────
builder.Services.AddScoped<CacheHelper>();

// ── JWT Authentication ────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// ── Caching ───────────────────────────────────────────
builder.Services.AddMemoryCache();

// ── Swagger ───────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartHealthcare API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── Seed Database ─────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        dbContext.Database.EnsureCreated();
        
        // Seed doctors if none exist
        if (!dbContext.Doctors.Any())
        {
            Log.Information("Seeding sample doctors...");
            
            // Create doctor users and doctors
            var salt = "smartHealthcare2024";
            
            var doctors = new[]
            {
                new { Email = "sarah.johnson@hospital.com", Name = "Sarah Johnson", SpecId = 1, Fee = 500m, Years = 8 },
                new { Email = "james.wilson@hospital.com", Name = "James Wilson", SpecId = 2, Fee = 400m, Years = 12 },
                new { Email = "emily.brown@hospital.com", Name = "Emily Brown", SpecId = 5, Fee = 350m, Years = 6 },
                new { Email = "michael.davis@hospital.com", Name = "Michael Davis", SpecId = 3, Fee = 450m, Years = 10 }
            };

            foreach (var doctorData in doctors)
            {
                // Check if user already exists
                if (!dbContext.Users.Any(u => u.Email == doctorData.Email))
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword("DoctorPass123!" + salt);
                    
                    var user = new SmartHealthcare.Models.Entities.User
                    {
                        FullName = doctorData.Name,
                        Email = doctorData.Email,
                        PasswordHash = hashedPassword,
                        Role = "Doctor",
                        CreatedAt = DateTime.UtcNow
                    };

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();

                    // Create doctor profile
                    var doctor = new SmartHealthcare.Models.Entities.Doctor
                    {
                        UserId = user.UserId,
                        Phone = $"+1-555-{1000 + dbContext.Doctors.Count():0000}",
                        Qualification = "MBBS, MD",
                        ExperienceYears = doctorData.Years,
                        ConsultationFee = doctorData.Fee,
                        IsAvailable = true
                    };

                    dbContext.Doctors.Add(doctor);
                    dbContext.SaveChanges();

                    // Link specialization
                    var doctorSpec = new SmartHealthcare.Models.Entities.DoctorSpecialization
                    {
                        DoctorId = doctor.DoctorId,
                        SpecializationId = doctorData.SpecId
                    };
                    dbContext.DoctorSpecializations.Add(doctorSpec);
                    dbContext.SaveChanges();
                    
                    Log.Information($"Created doctor: {doctorData.Name} ({doctorData.Email})");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error seeding database");
    }
}

// ── Middlewares ───────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("SmartHealthcare API starting...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}