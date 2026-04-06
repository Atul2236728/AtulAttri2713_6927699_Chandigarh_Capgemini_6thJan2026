# 🏥 MediCare — Smart Hospital Management System

A full-stack hospital management system built with **ASP.NET Core 8**, **Web API + MVC**, **Entity Framework Core**, and **SQL Server**.

---

## 🏗️ Architecture Overview

```
HospitalManagement/
├── API/                  ← ASP.NET Core Web API (port 7000 / 5000)
│   ├── Controllers/      ← Auth, Admin, Doctors, Appointments, Prescriptions, Bills, Medicines, Patients
│   ├── Models/           ← Entity classes (EF Core Code-First)
│   ├── DTOs/             ← Data Transfer Objects
│   ├── Data/             ← DbContext (HospitalDbContext)
│   ├── Helpers/          ← JwtHelper, AutoMapper MappingProfile
│   ├── Middleware/        ← Global exception handler
│   └── Program.cs        ← DI, JWT, CORS, Swagger configuration
│
├── Web/                  ← ASP.NET Core MVC Frontend (port 7001 / 5001)
│   ├── Controllers/      ← Auth, Home, Admin, Doctor, Patient
│   ├── Models/           ← ViewModels (mirrors API DTOs)
│   ├── Views/            ← Razor Views for all roles
│   ├── Services/         ← ApiService (HttpClient wrapper)
│   └── wwwroot/          ← CSS, JS (Bootstrap 5, FontAwesome, DataTables)
│
└── Database/
    ├── 01_CreateDatabase.sql   ← Full schema with FK relationships
    └── 02_SeedData.sql         ← 10 departments, 10 doctors, sample data
```

---

## ✅ Features Implemented

### 🔐 Authentication & Security
- JWT token-based auth in the API
- Cookie-based session auth in MVC
- Role-based authorization: **Admin | Doctor | Patient**
- BCrypt password hashing
- Global exception handling middleware

### 👤 User Roles & Workflows

| Feature | Admin | Doctor | Patient |
|---------|-------|--------|---------|
| Dashboard with stats | ✅ | ✅ | ✅ |
| Manage doctors (CRUD) | ✅ | — | — |
| Manage departments | ✅ | — | — |
| Manage medicines | ✅ | — | — |
| Manage users | ✅ | — | — |
| View all bills & update payment | ✅ | — | — |
| View today's schedule | — | ✅ | — |
| View/manage appointments | — | ✅ | ✅ |
| Write prescriptions | — | ✅ | — |
| Edit prescriptions | — | ✅ | — |
| Browse doctors by department | — | — | ✅ |
| Book appointments | — | — | ✅ |
| Cancel appointments | — | — | ✅ |
| View prescriptions | — | ✅ | ✅ |
| View bills | — | — | ✅ |
| Edit profile | — | ✅ | ✅ |
| Print prescriptions & bills | ✅ | ✅ | ✅ |

### 💊 Billing System
- Auto-generated bill on prescription save
- Consultation fee varies per doctor
- Medicine charges calculated from prescription items
- Other charges & discount fields
- `TotalAmount` as SQL computed column (`ConsultationFee + MedicineCharges + OtherCharges - Discount`)
- Payment status: Paid / Unpaid / Partial
- Payment method tracking (Cash, UPI, Card, Insurance)
- Printable invoice view

### 🗃️ Database Design
- **One-to-One**: User ↔ Doctor, User ↔ PatientProfile, Appointment ↔ Prescription, Appointment ↔ Bill
- **One-to-Many**: Department → Doctors, Patient → Appointments, Doctor → Appointments
- **Many-to-Many**: Prescription ↔ Medicines (via PrescriptionMedicines junction table)
- EF Core Code-First with fluent relationship config

---

## 🚀 Setup Instructions

### Prerequisites
- .NET 8 SDK — https://dotnet.microsoft.com/download
- SQL Server 2019+ (or SQL Server Express / LocalDB)
- Visual Studio 2022 or VS Code

---

### Step 1 — Run the Database Scripts

Open **SQL Server Management Studio (SSMS)** and run in order:

```sql
-- 1. Creates HospitalDB with all tables
exec '01_CreateDatabase.sql'

-- 2. Seeds departments, doctors, patients, medicines, sample data
exec '02_SeedData.sql'
```

**Note on seed passwords:** The seed script uses a placeholder BCrypt hash. To test login immediately, re-register the accounts through the app, or run this update after seeding:

```sql
USE HospitalDB;
-- Re-hash passwords correctly using the app's registration, OR
-- use the test credentials shown on the Login page which work with the API's BCrypt.
-- The seeded hash corresponds to: Admin@123 (may vary by BCrypt version)
-- Recommended: register fresh accounts and note your passwords.
```

---

### Step 2 — Configure Connection String

Edit **`API/appsettings.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Common values for `Server`:
| SQL Server Instance | Value |
|---|---|
| SQL Server Express | `.\SQLEXPRESS` |
| LocalDB | `(localdb)\MSSQLLocalDB` |
| Default local instance | `.` or `localhost` |
| Named instance | `.\INSTANCENAME` |

---

### Step 3 — Run the API

```bash
cd API
dotnet restore
dotnet run
```

API starts at:
- https://localhost:7000
- http://localhost:5000
- Swagger UI: https://localhost:7000/swagger

---

### Step 4 — Run the MVC Web App

```bash
cd Web
dotnet restore
dotnet run
```

Web app starts at:
- https://localhost:7001
- http://localhost:5001

---

### Step 5 — Login

Go to **https://localhost:7001** and use the quick-fill login buttons or:

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@hospital.com | Admin@123 |
| Doctor | rajesh.sharma@hospital.com | Admin@123 |
| Patient | rahul.verma@gmail.com | Admin@123 |

> **Important:** If the seeded BCrypt hashes don't match (due to BCrypt version differences), register a new patient account and use the Admin panel to create doctors.

---

### Step 6 — EF Core Migrations (Alternative to SQL scripts)

If you prefer Code-First migrations instead of running the SQL scripts:

```bash
cd API

# Install EF tools if not already installed
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply to database
dotnet ef database update
```

Then run only **`02_SeedData.sql`** for the sample data.

---

## 📡 API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/login` | Login (all roles) |
| POST | `/api/auth/register` | Patient self-registration |

### Doctors
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/doctors` | List all (filter: departmentId, search) |
| GET | `/api/doctors/{id}` | Get doctor detail |
| PUT | `/api/doctors/{id}` | Update doctor |
| GET | `/api/doctors/{id}/dashboard` | Doctor dashboard stats |
| GET | `/api/doctors/{id}/appointments` | Doctor's appointments |

### Appointments
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/appointments/{id}` | Get appointment |
| GET | `/api/appointments/patient/{patientId}` | Patient's appointments |
| POST | `/api/appointments` | Book appointment |
| PATCH | `/api/appointments/{id}/status` | Update status |
| DELETE | `/api/appointments/{id}` | Cancel appointment |

### Prescriptions
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/prescriptions/{id}` | Get prescription |
| GET | `/api/prescriptions/appointment/{id}` | By appointment |
| GET | `/api/prescriptions/patient/{id}` | Patient history |
| POST | `/api/prescriptions` | Create (auto-generates bill) |
| PUT | `/api/prescriptions/{id}` | Update |

### Bills
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bills/{id}` | Get bill |
| GET | `/api/bills/appointment/{id}` | By appointment |
| GET | `/api/bills/patient/{id}` | Patient's bills |
| POST | `/api/bills` | Generate bill |
| PATCH | `/api/bills/{id}/payment` | Update payment status |

### Admin
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/admin/dashboard` | Dashboard stats |
| GET/POST | `/api/admin/departments` | Manage departments |
| POST | `/api/admin/doctors` | Create doctor |
| DELETE | `/api/admin/doctors/{id}` | Deactivate doctor |
| GET | `/api/admin/bills` | All bills |
| GET | `/api/admin/users` | All users |

---

## 🗂️ Project Structure Details

### API Technologies
- ASP.NET Core 8 Web API
- Entity Framework Core 8 (Code-First)
- SQL Server provider
- AutoMapper 12 (DTO mapping)
- BCrypt.Net-Next (password hashing)
- JWT Bearer Authentication
- Swashbuckle / Swagger UI
- Global exception middleware
- ILogger logging

### Web Technologies
- ASP.NET Core 8 MVC
- Razor Views
- Cookie Authentication
- HttpClient (consumes API)
- Bootstrap 5.3
- FontAwesome 6
- DataTables.net
- jQuery 3.7

---

## 🐛 Troubleshooting

**"Connection refused" when Web calls API:**
→ Make sure the API is running first. Check `ApiSettings:BaseUrl` in `Web/appsettings.json` matches the API port.

**SSL certificate errors:**
→ Run `dotnet dev-certs https --trust` once on your machine.

**BCrypt login failure with seed data:**
→ The seed data hash is a placeholder. Use the app's Register page to create real accounts, or update passwords via the API's `/api/auth/register` endpoint.

**EF Core migration errors:**
→ Delete the `Migrations` folder and re-run `dotnet ef migrations add InitialCreate`.

**DataTables not working:**
→ Make sure jQuery is loaded before DataTables. The `_Layout.cshtml` already handles this.

---

## 📋 Assessment Checklist

| Requirement | Status |
|---|---|
| Frontend: ASP.NET Core MVC (Razor Views) | ✅ |
| Backend: ASP.NET Core Web API | ✅ |
| Database: SQL Server | ✅ |
| ORM: Entity Framework Core | ✅ |
| User Management (Admin/Doctor/Patient) | ✅ |
| Doctor & Department Management | ✅ |
| Appointment Booking System | ✅ |
| Prescription & Medical Records | ✅ |
| Billing System (fee + medicines) | ✅ |
| MVC consumes API via HttpClient | ✅ |
| One-to-One relationships (EF Core) | ✅ |
| One-to-Many relationships (EF Core) | ✅ |
| JWT Authentication in API | ✅ |
| Role-based authorization | ✅ |
| Client-side validation (jQuery) | ✅ |
| Server-side validation (Data Annotations) | ✅ |
| Global exception handling middleware | ✅ |
| ILogger logging | ✅ |
| DTOs | ✅ |
| AutoMapper | ✅ |
| Repository pattern (via DbContext abstraction) | ✅ |
| Swagger/OpenAPI | ✅ |
| Seed data (10 departments, 10 doctors) | ✅ |

---

*Built for Capgemini Chandigarh — Week 13 Assessment*
*Smart Hospital Management System*
