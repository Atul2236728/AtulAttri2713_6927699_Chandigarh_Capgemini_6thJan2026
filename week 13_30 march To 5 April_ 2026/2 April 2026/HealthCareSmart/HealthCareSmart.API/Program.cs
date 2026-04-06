using HealthCareSmart.API.Data;
using HealthCareSmart.API.Mappings;
using HealthCareSmart.API.Middleware;
using HealthCareSmart.API.Services;
using HealthCareSmart.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/healthcaresmart-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<AuthService>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthCareSmart API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token here}"
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

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed default users if none exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    db.Database.Migrate();

    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'dbo.Doctors', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Doctors', N'ConsultationFee') IS NULL
BEGIN
    ALTER TABLE dbo.Doctors ADD ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0;
END
IF OBJECT_ID(N'dbo.Medicines', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Medicines', N'Price') IS NULL
BEGIN
    ALTER TABLE dbo.Medicines ADD Price DECIMAL(18,2) NOT NULL DEFAULT 0;
END
IF OBJECT_ID(N'dbo.Billings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Billings (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        AppointmentId INT NOT NULL,
        ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0,
        MedicineCost DECIMAL(18,2) NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_Billings_Appointments FOREIGN KEY (AppointmentId) REFERENCES dbo.Appointments(Id)
    );
END
ELSE IF COL_LENGTH(N'dbo.Billings', N'ConsultationFee') IS NULL
BEGIN
    ALTER TABLE dbo.Billings ADD ConsultationFee DECIMAL(18,2) NOT NULL DEFAULT 0;
END
ELSE IF COL_LENGTH(N'dbo.Billings', N'MedicineCost') IS NULL
BEGIN
    ALTER TABLE dbo.Billings ADD MedicineCost DECIMAL(18,2) NOT NULL DEFAULT 0;
END
");

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User
            {
                FullName = "Admin User",
                Email = "admin@healthcare.com",
                PasswordHash = "Admin123",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FullName = "Dr. John",
                Email = "doctor@healthcare.com",
                PasswordHash = "Doctor123",
                Role = "Doctor",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FullName = "Dr. Alice",
                Email = "doctor1@healthcare.com",
                PasswordHash = "Doctor123",
                Role = "Doctor",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FullName = "Dr. Bob",
                Email = "doctor2@healthcare.com",
                PasswordHash = "Doctor123",
                Role = "Doctor",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FullName = "Dr. Clara",
                Email = "doctor3@healthcare.com",
                PasswordHash = "Doctor123",
                Role = "Doctor",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FullName = "Dr. David",
                Email = "doctor4@healthcare.com",
                PasswordHash = "Doctor123",
                Role = "Doctor",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                FullName = "Jane Patient",
                Email = "patient@healthcare.com",
                PasswordHash = "Patient123",
                Role = "Patient",
                CreatedAt = DateTime.UtcNow
            }
        );
        db.SaveChanges();
        Log.Information("Database seeded with default users.");
    }

    if (!db.Patients.Any())
    {
        var patientUser = db.Users.FirstOrDefault(u => u.Email == "patient@healthcare.com");
        if (patientUser != null)
        {
            db.Patients.Add(new Patient
            {
                PhoneNumber = "9998887770",
                DateOfBirth = new DateTime(1992, 7, 15),
                Address = "123 Main Street, Health City",
                UserId = patientUser.Id
            });
            db.SaveChanges();
            Log.Information("Database seeded with default patient profile.");
        }
    }

    if (!db.DoctorSpecializations.Any())
    {
        var cardiology = new DoctorSpecialization { Name = "Cardiology" };
        var orthopedics = new DoctorSpecialization { Name = "Orthopedics" };
        var dermatology = new DoctorSpecialization { Name = "Dermatology" };
        var neurology = new DoctorSpecialization { Name = "Neurology" };
        var pediatrics = new DoctorSpecialization { Name = "Pediatrics" };
        var generalMedicine = new DoctorSpecialization { Name = "General Medicine" };
        var gynecology = new DoctorSpecialization { Name = "Gynecology" };
        var endocrinology = new DoctorSpecialization { Name = "Endocrinology" };
        var ent = new DoctorSpecialization { Name = "ENT" };
        var psychiatry = new DoctorSpecialization { Name = "Psychiatry" };
        var radiology = new DoctorSpecialization { Name = "Radiology" };
        var ophthalmology = new DoctorSpecialization { Name = "Ophthalmology" };
        var preventiveMedicine = new DoctorSpecialization { Name = "Preventive Medicine" };

        db.DoctorSpecializations.AddRange(cardiology, orthopedics, dermatology, neurology, pediatrics,
            generalMedicine, gynecology, endocrinology, ent, psychiatry, radiology, ophthalmology, preventiveMedicine);
        db.SaveChanges();

        db.Doctors.AddRange(
            new Doctor
            {
                FullName = "Dr. Asha Patel",
                PhoneNumber = "9876543210",
                ConsultationFee = 1200m,
                Specializations = new List<DoctorSpecialization> { cardiology }
            },
            new Doctor
            {
                FullName = "Dr. Vivek Sharma",
                PhoneNumber = "9876501234",
                ConsultationFee = 950m,
                Specializations = new List<DoctorSpecialization> { orthopedics }
            },
            new Doctor
            {
                FullName = "Dr. Meera Das",
                PhoneNumber = "9876512345",
                ConsultationFee = 850m,
                Specializations = new List<DoctorSpecialization> { dermatology }
            },
            new Doctor
            {
                FullName = "Dr. Sandeep Rao",
                PhoneNumber = "9876523456",
                ConsultationFee = 1500m,
                Specializations = new List<DoctorSpecialization> { neurology }
            },
            new Doctor
            {
                FullName = "Dr. Priya Nair",
                PhoneNumber = "9876534567",
                ConsultationFee = 700m,
                Specializations = new List<DoctorSpecialization> { pediatrics, generalMedicine }
            },
            new Doctor
            {
                FullName = "Dr. Rohit Gupta",
                PhoneNumber = "9876545678",
                ConsultationFee = 650m,
                Specializations = new List<DoctorSpecialization> { generalMedicine }
            },
            new Doctor
            {
                FullName = "Dr. Anita Singh",
                PhoneNumber = "9876556789",
                ConsultationFee = 1100m,
                Specializations = new List<DoctorSpecialization> { gynecology }
            },
            new Doctor
            {
                FullName = "Dr. Arjun Mehta",
                PhoneNumber = "9876567890",
                ConsultationFee = 1300m,
                Specializations = new List<DoctorSpecialization> { endocrinology }
            },
            new Doctor
            {
                FullName = "Dr. Kavita Joshi",
                PhoneNumber = "9876578901",
                ConsultationFee = 750m,
                Specializations = new List<DoctorSpecialization> { ent }
            },
            new Doctor
            {
                FullName = "Dr. Neha Kapoor",
                PhoneNumber = "9876589012",
                ConsultationFee = 1000m,
                Specializations = new List<DoctorSpecialization> { psychiatry }
            },
            new Doctor
            {
                FullName = "Dr. Rakesh Jain",
                PhoneNumber = "9876590123",
                ConsultationFee = 1400m,
                Specializations = new List<DoctorSpecialization> { radiology }
            },
            new Doctor
            {
                FullName = "Dr. Nisha Verma",
                PhoneNumber = "9876591234",
                ConsultationFee = 900m,
                Specializations = new List<DoctorSpecialization> { ophthalmology }
            },
            new Doctor
            {
                FullName = "Dr. Aruna Gupta",
                PhoneNumber = "9876592345",
                ConsultationFee = 800m,
                Specializations = new List<DoctorSpecialization> { preventiveMedicine, generalMedicine }
            },
            new Doctor
            {
                FullName = "Dr. Sunita Rao",
                PhoneNumber = "9876601234",
                ConsultationFee = 1100m,
                Specializations = new List<DoctorSpecialization> { cardiology, generalMedicine }
            },
            new Doctor
            {
                FullName = "Dr. Neel Kapoor",
                PhoneNumber = "9876612345",
                ConsultationFee = 1250m,
                Specializations = new List<DoctorSpecialization> { cardiology }
            },
            new Doctor
            {
                FullName = "Dr. Tanya Arora",
                PhoneNumber = "9876623456",
                ConsultationFee = 900m,
                Specializations = new List<DoctorSpecialization> { dermatology }
            },
            new Doctor
            {
                FullName = "Dr. Kiran Nanda",
                PhoneNumber = "9876634567",
                ConsultationFee = 1450m,
                Specializations = new List<DoctorSpecialization> { neurology }
            },
            new Doctor
            {
                FullName = "Dr. Rajesh Verma",
                PhoneNumber = "9876645678",
                ConsultationFee = 800m,
                Specializations = new List<DoctorSpecialization> { ent }
            },
            new Doctor
            {
                FullName = "Dr. Ritu Shah",
                PhoneNumber = "9876656789",
                ConsultationFee = 950m,
                Specializations = new List<DoctorSpecialization> { ophthalmology }
            },
            new Doctor
            {
                FullName = "Dr. Maya Mishra",
                PhoneNumber = "9876667890",
                ConsultationFee = 1050m,
                Specializations = new List<DoctorSpecialization> { gynecology }
            },
            new Doctor
            {
                FullName = "Dr. Sneha Kapoor",
                PhoneNumber = "9876678901",
                ConsultationFee = 1350m,
                Specializations = new List<DoctorSpecialization> { radiology }
            },
            new Doctor
            {
                FullName = "Dr. Vikram Jain",
                PhoneNumber = "9876689012",
                ConsultationFee = 850m,
                Specializations = new List<DoctorSpecialization> { preventiveMedicine }
            }
        );
        db.SaveChanges();

        // Seed medicines for different specializations
        var medicines = new List<Medicine>
        {
            // Cardiology medicines
            new Medicine { Name = "Aspirin", Dosage = "100mg", Price = 150m },
            new Medicine { Name = "Atorvastatin", Dosage = "20mg", Price = 250m },
            new Medicine { Name = "Lisinopril", Dosage = "10mg", Price = 180m },
            new Medicine { Name = "Metoprolol", Dosage = "50mg", Price = 200m },
            
            // Orthopedics medicines
            new Medicine { Name = "Ibuprofen", Dosage = "400mg", Price = 80m },
            new Medicine { Name = "Diclofenac", Dosage = "50mg", Price = 120m },
            new Medicine { Name = "Paracetamol", Dosage = "500mg", Price = 60m },
            new Medicine { Name = "Muscle Relaxant - Cyclobenzaprine", Dosage = "5mg", Price = 200m },
            
            // Dermatology medicines
            new Medicine { Name = "Hydrocortisone Cream", Dosage = "1%", Price = 300m },
            new Medicine { Name = "Clotrimazole", Dosage = "1%", Price = 250m },
            new Medicine { Name = "Tretinoin Cream", Dosage = "0.025%", Price = 450m },
            new Medicine { Name = "Cetirizine", Dosage = "10mg", Price = 100m },
            
            // Neurology medicines
            new Medicine { Name = "Levetiracetam", Dosage = "500mg", Price = 350m },
            new Medicine { Name = "Gabapentin", Dosage = "300mg", Price = 280m },
            new Medicine { Name = "Phenytoin", Dosage = "100mg", Price = 220m },
            new Medicine { Name = "Valproic Acid", Dosage = "250mg", Price = 380m },
            
            // Pediatrics medicines
            new Medicine { Name = "Amoxicillin", Dosage = "250mg/5ml", Price = 140m },
            new Medicine { Name = "Ibuprofen Syrup", Dosage = "100mg/5ml", Price = 90m },
            new Medicine { Name = "Vitamin D3", Dosage = "1000 IU", Price = 120m },
            new Medicine { Name = "Zinc Supplement", Dosage = "10mg", Price = 110m },
            
            // General Medicine medicines
            new Medicine { Name = "Metformin", Dosage = "500mg", Price = 160m },
            new Medicine { Name = "Enalapril", Dosage = "5mg", Price = 190m },
            new Medicine { Name = "Omeprazole", Dosage = "20mg", Price = 170m },
            new Medicine { Name = "Amoxicillin", Dosage = "500mg", Price = 200m },
            
            // Gynecology medicines
            new Medicine { Name = "Mefenamic Acid", Dosage = "500mg", Price = 140m },
            new Medicine { Name = "Nifedipine", Dosage = "10mg", Price = 250m },
            new Medicine { Name = "Folic Acid", Dosage = "5mg", Price = 80m },
            new Medicine { Name = "Iron Supplement", Dosage = "325mg", Price = 100m },
            
            // Endocrinology medicines
            new Medicine { Name = "Insulin Glargine", Dosage = "100 IU/ml", Price = 800m },
            new Medicine { Name = "Levothyroxine", Dosage = "50mcg", Price = 180m },
            new Medicine { Name = "Pioglitazone", Dosage = "15mg", Price = 320m },
            new Medicine { Name = "Sitagliptin", Dosage = "50mg", Price = 400m },
            
            // ENT medicines
            new Medicine { Name = "Amoxicillin-Clavulanate", Dosage = "625mg", Price = 280m },
            new Medicine { Name = "Fluticasone Nasal Spray", Dosage = "50mcg", Price = 350m },
            new Medicine { Name = "Cetirizine HCl", Dosage = "10mg", Price = 100m },
            new Medicine { Name = "Chlorpheniramine", Dosage = "4mg", Price = 90m },
            
            // Psychiatry medicines
            new Medicine { Name = "Sertraline", Dosage = "50mg", Price = 280m },
            new Medicine { Name = "Fluoxetine", Dosage = "20mg", Price = 300m },
            new Medicine { Name = "Alprazolam", Dosage = "0.5mg", Price = 250m },
            new Medicine { Name = "Olanzapine", Dosage = "5mg", Price = 450m },
            
            // Radiology medicines (contrast agents and supportive care)
            new Medicine { Name = "Barium Sulfate", Dosage = "100%", Price = 500m },
            new Medicine { Name = "Iodinated Contrast", Dosage = "370mg/ml", Price = 1200m },
            new Medicine { Name = "Hydration Saline", Dosage = "0.9%", Price = 150m },
            new Medicine { Name = "Metformin Hold Instruction", Dosage = "Protocol", Price = 0m },
            
            // Ophthalmology medicines
            new Medicine { Name = "Timolol Eye Drops", Dosage = "0.5%", Price = 280m },
            new Medicine { Name = "Pilocarpine", Dosage = "2%", Price = 200m },
            new Medicine { Name = "Ketorolac Eye Drops", Dosage = "0.4%", Price = 320m },
            new Medicine { Name = "Lubricating Eye Drops", Dosage = "Artificial Tears", Price = 120m },
            
            // Preventive Medicine
            new Medicine { Name = "Atorvastatin", Dosage = "20mg", Price = 250m },
            new Medicine { Name = "Low Dose Aspirin", Dosage = "81mg", Price = 120m },
            new Medicine { Name = "Vitamin B Complex", Dosage = "Complex", Price = 180m },
            new Medicine { Name = "CoQ10 Supplement", Dosage = "100mg", Price = 350m }
        };

        db.Medicines.AddRange(medicines);
        db.SaveChanges();
        Log.Information("Database seeded with medicines for all specializations.");
        Log.Information("Database seeded with default doctors and specialties.");
    }
}

app.Run();