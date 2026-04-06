# SmartHealthcare Deployment Checklist

## Pre-Deployment & Verification

### Phase 1: Build & Compilation ✅
- [x] Solution builds without errors
- [x] No critical compiler warnings
- [x] All NuGet packages restored successfully
- [x] Project references correct

### Phase 2: Database Setup
- [ ] Run migrations to SQL Server:
  ```bash
  cd src/SmartHealthcare.API
  dotnet ef database update
  ```
- [ ] Verify database `SmartHealthCareDB` created on `Blackpearl\SQLEXPRESS`
- [ ] Confirm seed data populated:
  - [ ] 6 specializations seeded
  - [ ] 1 admin user created (admin@healthcare.com)
  - [ ] 5 doctors created with specializations
  - [ ] 5 patients created with profiles
  - [ ] 4 appointments created (mix of Pending/Completed)
  - [ ] Prescriptions with medicines seeded

### Phase 3: Configuration Verification
- [ ] Check `appsettings.json` settings:
  - [ ] Database connection string correct
  - [ ] JWT secret key configured
  - [ ] JWT expiry times set (60 min access, 7 days refresh)
  - [ ] Logging path configured
  - [ ] CORS origins set correctly
  
- [ ] Check `appsettings.Development.json` for local testing

### Phase 4: Backend API Testing
- [ ] API starts successfully: `dotnet run` in API project
- [ ] API accessible on `https://localhost:5293`
- [ ] Swagger documentation available at `/swagger`
- [ ] Health check endpoint responds: `GET /api/health`

**API Endpoints Verification**:
- [ ] Auth endpoints working:
  - [ ] `POST /api/auth/register` - New user registration
  - [ ] `POST /api/auth/login` - User login
  - [ ] `POST /api/auth/refresh-token` - Token refresh
  - [ ] `POST /api/auth/logout` - User logout

- [ ] Patient endpoints working:
  - [ ] `GET /api/patients` - List patients (with pagination)
  - [ ] `GET /api/patients/{id}` - Patient details
  - [ ] `POST /api/patients` - Create patient profile
  - [ ] `PUT /api/patients/{id}` - Update patient

- [ ] Doctor endpoints working:
  - [ ] `GET /api/doctors` - List doctors (with pagination)
  - [ ] `GET /api/doctors/{id}` - Doctor details
  - [ ] `GET /api/doctors/specialization/{id}` - Doctors by specialty

- [ ] Appointment endpoints working:
  - [ ] `POST /api/appointments` - Book appointment
  - [ ] `GET /api/appointments` - User's appointments
  - [ ] `PUT /api/appointments/{id}` - Update appointment
  - [ ] `DELETE /api/appointments/{id}` - Cancel appointment

- [ ] Prescription endpoints working:
  - [ ] `GET /api/prescriptions/{id}` - Get prescription
  - [ ] `POST /api/prescriptions` - Create prescription

- [ ] Admin endpoints working:
  - [ ] `GET /api/admin/users` - List users
  - [ ] `POST /api/admin/users` - Create user
  - [ ] `DELETE /api/admin/users/{id}` - Delete user

### Phase 5: Frontend MVC Testing
- [ ] MVC application starts: `dotnet run` in MVC project
- [ ] MVC accessible on `https://localhost:5013`
- [ ] Static files loading (CSS, JS, images)
- [ ] All Razor views rendering without errors

**Page Verification**:
- [ ] Home page: `/` or `/Home/Index`
- [ ] Login page: `/Account/Login`
- [ ] Registration page: `/Account/Register`
- [ ] Dashboard: `/Dashboard/Index` (role-based)
- [ ] Doctor listing: `/Doctor/Index`
- [ ] Doctor details: `/Doctor/Details/{id}`
- [ ] Appointments section: `/Appointment/Index`
- [ ] Patient profile: `/Patient/Profile`
- [ ] Admin panel (Users): `/Admin/Users`
- [ ] Admin panel (Reports): `/Admin/Reports`

### Phase 6: Authentication Flow Testing
- [ ] Fresh user registration works end-to-end
- [ ] Login with registered credentials
- [ ] JWT token generated and stored
- [ ] Dashboard displays based on user role
- [ ] Token refresh mechanism works
- [ ] Logout clears session/token
- [ ] Role-based access control enforced
- [ ] Invalid credentials rejected

**Test Credentials** (from seed data):
```
Admin:     admin@healthcare.com / Admin@123
Doctor:    doctor1@healthcare.com / Doctor@123
Patient:   patient1@healthcare.com / Patient@123
```

### Phase 7: Feature Testing

**Patient Features**:
- [ ] Browse available doctors
- [ ] Filter doctors by specialization
- [ ] Book appointment with dates/times
- [ ] View upcoming appointments
- [ ] View completed appointments
- [ ] View prescriptions from appointments
- [ ] Update personal medical profile

**Doctor Features**:
- [ ] View assigned appointments
- [ ] See patient details for appointments
- [ ] Create prescriptions for patients
- [ ] Add medicines to prescriptions

**Admin Features**:
- [ ] View all users
- [ ] Create new users
- [ ] Delete users
- [ ] View system statistics/reports
- [ ] See appointment analytics

### Phase 8: Error Handling & Edge Cases
- [ ] Invalid endpoints return 404
- [ ] Missing required fields return 400
- [ ] Unauthorized requests return 401
- [ ] Forbidden access returns 403
- [ ] Server errors return 500 with generic message
- [ ] No stack traces exposed in production
- [ ] Validation messages clear and helpful

### Phase 9: Performance & Load
- [ ] Page load times < 2 seconds
- [ ] API responses < 1 second (normal operations)
- [ ] Doctor list loads quickly (pagination works)
- [ ] Appointment list handles 100+ records
- [ ] Search/filter operations responsive

### Phase 10: Security Checks
- [ ] Passwords hashed with BCrypt
- [ ] JWT tokens signed with secret key
- [ ] HTTPS enforced (localhost for dev)
- [ ] CORS properly configured
- [ ] SQL injection prevented (parameterized queries)
- [ ] XSS prevention (model validation)
- [ ] CSRF tokens in forms (if applicable)
- [ ] No sensitive data in URL parameters
- [ ] Session timeout implemented

### Phase 11: Logging & Monitoring
- [ ] Logs directory exists: `src/SmartHealthcare.API/Logs/`
- [ ] Serilog configured and writing logs
- [ ] Log files rotating daily
- [ ] Exception details logged (not exposed to users)
- [ ] API calls logged (request/response)
- [ ] Performance metrics available

### Phase 12: Documentation
- [ ] [✅] QUICK_START.md created
- [ ] [✅] PROJECT_SUMMARY.md created
- [ ] [✅] IMPLEMENTATION_GUIDE.md created
- [ ] [✅] AUTHENTICATION_TEST_GUIDE.md created
- [ ] ReadMe.md updated with project overview

---

## Deployment Steps

### For Local/Staging Deployment:

1. **Clone/Pull Latest Code**
   ```bash
   git clone <repo-url>
   cd SmartHealthCare
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Apply Database Migrations**
   ```bash
   cd src/SmartHealthcare.API
   dotnet ef database update
   ```

4. **Verify Configuration**
   - Update connection strings if needed
   - Update API/MVC URLs in configs
   - Set environment variables

5. **Build Solution**
   ```bash
   cd ..\..\
   dotnet build
   ```

6. **Run API**
   ```bash
   cd src/SmartHealthcare.API
   dotnet run
   # API runs on https://localhost:5293
   ```

7. **Run MVC** (in new terminal)
   ```bash
   cd src/SmartHealthcare.MVC
   dotnet run
   # MVC runs on https://localhost:5013
   ```

8. **Verify Accessibility**
   - Open http://localhost:5013 in browser
   - Verify home page loads
   - Test login with: admin@healthcare.com / Admin@123

### For Production Deployment:

1. **Update Configuration**
   - Use `appsettings.Production.json`
   - Set production database connection
   - Configure JWT secret key (use environment variable)
   - Set HTTPS + SSL certificates

2. **Build Release**
   ```bash
   dotnet publish -c Release
   ```

3. **Database Setup**
   - Apply migrations to production database
   - Run seed data (recommended for initial setup)

4. **Deploy using**:
   - IIS (Windows hosting)
   - Docker containers
   - Azure App Service
   - Other hosting platform

5. **Post-Deployment**
   - Verify all endpoints accessible
   - Test critical authentication flows
   - Monitor logs for errors
   - Set up automated backups

---

## Post-Deployment Validation

### Day 1 Checks:
- [ ] All users can register successfully
- [ ] Login works for all roles
- [ ] Appointments can be booked
- [ ] Doctors can view patients
- [ ] Admin can manage users
- [ ] No errors in logs

### Week 1 Checks:
- [ ] ~10+ users registered
- [ ] ~20+ appointments created
- [ ] System stable (no crashes)
- [ ] Performance acceptable
- [ ] No security incidents

### Monthly Checks:
- [ ] Database backups verified
- [ ] Logs reviewed for patterns
- [ ] Performance metrics analyzed
- [ ] Security updates applied
- [ ] User feedback collected

---

## Rollback Plan

If critical issues found:

1. **Stop Applications**
2. **Restore Previous Database Backup**
   ```sql
   -- SQL Server restore
   RESTORE DATABASE SmartHealthCareDB 
   FROM DISK = 'C:\backups\SmartHealthCareDB_backup.bak'
   ```
3. **Redeploy Previous Version**
4. **Notify Users**
5. **Investigate Root Cause**
6. **Fix & Re-test Before Re-deployment**

---

## Sign-Off

- [ ] Development Lead: __________  Date: __________
- [ ] QA Lead: __________  Date: __________
- [ ] DevOps/Deployment: __________  Date: __________
- [ ] Product Owner: __________  Date: __________

---

## Notes for Deployment Team

1. **Database Size**: Initially small (<1MB), will grow with data
2. **Backup Strategy**: Daily backups recommended
3. **Monitoring**: Set up alerts for:
   - Application restarts
   - Errors in logs
   - High response times (>2 sec)
   - Failed authentication attempts
4. **Scaling**: If needed, implement caching for doctor list
5. **Support Contact**: __________

