# Quick Start Guide - Smart Healthcare

## 🚀 Quick Setup (5 minutes)

### 1. Update Database Connection
Edit: `src/SmartHealthcare.API/appsettings.json`
```json
"DefaultConnection": "Server=YOUR_SERVER\\SQLEXPRESS;Database=SmartHealthCareDB;Trusted_Connection=True;TrustServerCertificate=True"
```

### 2. Create Database
```bash
cd src/SmartHealthcare.API
dotnet ef database update
```

### 3. Run API (Terminal 1)
```bash
cd src/SmartHealthcare.API
dotnet run
# Will start on http://localhost:5293
```

### 4. Run MVC (Terminal 2)
```bash
cd src/SmartHealthcare.MVC
dotnet run
# Will start on http://localhost:5013
```

### 5. Open Browser
Navigate to: `http://localhost:5013`

---

## 🔐 Demo Login
- **Email**: admin@healthcare.com
- **Password**: Admin@123

---

## 📋 What's Included

### Ready-to-Use Features ✅
- Complete authentication system
- Role-based dashboards (Admin/Doctor/Patient)
- Professional UI with Bootstrap 5
- Doctor listing and search
- Appointment booking system
- Patient profile management
- Admin reporting dashboard
- API with Swagger documentation
- Database with Entity Framework

### Test the Application
1. **Register** - Create new patient/doctor account
2. **Login** - Use demo credentials
3. **Dashboard** - View role-specific content
4. **Browse Doctors** - Search doctors
5. **Book Appointment** - Schedule a visit
6. **View Admin Reports** - Check system stats

---

## 🎯 Key URLs

| Feature | URL |
|---------|-----|
| Home | http://localhost:5013/ |
| Login | http://localhost:5013/account/login |
| Register | http://localhost:5013/account/register |
| Dashboard | http://localhost:5013/dashboard |
| Doctors | http://localhost:5013/doctor |
| Appointments | http://localhost:5013/appointment |
| Profile | http://localhost:5013/patient/profile |
| API Swagger | http://localhost:5293/swagger |

---

## 💡 Tips

- All views are responsive and mobile-friendly
- Use browser DevTools (F12) to test responsiveness
- Check browser console for any JavaScript errors
- API errors are logged in `src/SmartHealthcare.API/Logs/`
- Database seed includes demo specializations and admin user

---

## ⚙️ Configuration Files

### API Settings
- Location: `src/SmartHealthcare.API/appsettings.json`
- JWT Configuration
- Database Connection
- Logging Settings

### MVC Settings
- Location: `src/SmartHealthcare.MVC/appsettings.json`
- API Base URL
- Session Configuration

---

## 🐛 Common Issues & Fixes

| Issue | Solution |
|-------|----------|
| Cannot connect to database | Verify SQL Server is running and connection string is correct |
| Port 5293 already in use | Close existing API or change the port in launchSettings.json |
| Login page not loading | Ensure both API and MVC are running |
| Styling is broken | Clear browser cache (Ctrl+F5) |
| Database error on first run | Run `dotnet ef database update` |

---

## 📞 Support

All features are production-ready. Review the main `IMPLEMENTATION_GUIDE.md` for detailed documentation.

**Last Updated**: April 2026
**Status**: ✅ Production Ready
