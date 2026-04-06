# SmartHealthcare Authentication Testing Guide

## Overview
This guide documents all authentication flows that should be tested to ensure the system is fully functional.

## Test Environment Setup

### Prerequisites
1. Database should be updated with latest migrations:
   ```bash
   cd src/SmartHealthcare.API
   dotnet ef database update
   ```

2. Both applications should be running:
   - **API**: `dotnet run` in `src/SmartHealthcare.API` → runs on `https://localhost:5293`
   - **MVC**: `dotnet run` in `src/SmartHealthcare.MVC` → runs on `https://localhost:5013`

### Test Credentials
After running migrations with seed data, use these credentials:

| Role | Email | Password | Purpose |
|------|-------|----------|---------|
| Admin | admin@healthcare.com | Admin@123 | System administration |
| Doctor 1 | doctor1@healthcare.com | Doctor@123 | Cardiology specialist |
| Doctor 2 | doctor2@healthcare.com | Doctor@123 | Dermatology specialist |
| Doctor 3 | doctor3@healthcare.com | Doctor@123 | Orthopedics specialist |
| Doctor 4 | doctor4@healthcare.com | Doctor@123 | Pediatrics specialist |
| Doctor 5 | doctor5@healthcare.com | Doctor@123 | Neurology specialist |
| Patient 1 | patient1@healthcare.com | Patient@123 | Test patient (Raj Patel) |
| Patient 2 | patient2@healthcare.com | Patient@123 | Test patient (Anjali Verma) |
| Patient 3 | patient3@healthcare.com | Patient@123 | Test patient (Amit Kumar) |
| Patient 4 | patient4@healthcare.com | Patient@123 | Test patient (Sneha Gupta) |
| Patient 5 | patient5@healthcare.com | Patient@123 | Test patient (Arjun Desai) |

---

## Test Cases

### Test 1: User Registration (New Patient)
**Objective**: Verify new users can register and receive JWT token

**Steps**:
1. Navigate to `https://localhost:5013/Account/Register`
2. Fill in registration form:
   - Full Name: "John Doe"
   - Email: "john.doe@test.com"
   - Password: "NewUser@123"
   - Confirm Password: "NewUser@123"
   - Role: Select "Patient"
3. Click "Register"

**Expected Results**:
- ✅ Registration succeeds
- ✅ Redirected to Patient Profile page
- ✅ JWT token stored in session/browser storage
- ✅ User can see profile creation form

**Failure Indicators**:
- ❌ Server error on registration
- ❌ Password validation fails without clear message
- ❌ Redirect loop or missing page

---

### Test 2: Patient Login and Dashboard Access
**Objective**: Verify patient can login and see patient-specific dashboard

**Steps**:
1. Navigate to `https://localhost:5013/Account/Login`
2. Enter credentials:
   - Email: `patient1@healthcare.com`
   - Password: `Patient@123`
3. Click "Login"

**Expected Results**:
- ✅ Login succeeds
- ✅ Redirected to Dashboard
- ✅ Dashboard shows "Patient Dashboard" with:
  - Appointment booking section
  - Prescription history
  - Medical profile information
- ✅ Navigation menu shows patient-specific options

**Verification Commands** (Postman/curl):
```bash
curl -X POST https://localhost:5293/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "patient1@healthcare.com",
    "password": "Patient@123"
  }'
```

Expected response:
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "...",
    "user": {
      "id": "...",
      "fullName": "Raj Patel",
      "email": "patient1@healthcare.com",
      "role": "Patient"
    }
  }
}
```

---

### Test 3: Doctor Login and Appointment Viewing
**Objective**: Verify doctor can login and see their appointments

**Steps**:
1. Navigate to `https://localhost:5013/Account/Login`
2. Enter credentials:
   - Email: `doctor1@healthcare.com`
   - Password: `Doctor@123`
3. Click "Login"

**Expected Results**:
- ✅ Login succeeds
- ✅ Redirected to Doctor Dashboard
- ✅ Dashboard displays:
  - Assigned appointments
  - Patient list
  - Prescription management section
- ✅ Can view appointment details and assigned patients

**API Verification** (with JWT token):
```bash
curl -X GET https://localhost:5293/api/appointments/doctor \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

---

### Test 4: Admin Login and User Management
**Objective**: Verify admin can access admin panel

**Steps**:
1. Navigate to `https://localhost:5013/Account/Login`
2. Enter credentials:
   - Email: `admin@healthcare.com`
   - Password: `Admin@123`
3. Click "Login"

**Expected Results**:
- ✅ Login succeeds
- ✅ Redirected to Admin Dashboard
- ✅ Admin dashboard shows:
  - User management section (Users list)
  - System reports and statistics
  - User creation/deletion/role assignment options

---

### Test 5: Role-Based Access Control (RBAC)
**Objective**: Verify unauthorized users cannot access restricted pages

**Steps**:
1. Login as patient (`patient1@healthcare.com`)
2. Try to access admin panel: `https://localhost:5013/Admin/Users`
3. Try to access doctor dashboard: `https://localhost:5013/Doctor/Index`

**Expected Results**:
- ✅ Access denied
- ✅ Redirected to home page or access denied page
- ✅ No error messages exposed in response

**API Verification** (without authorized role):
```bash
# Attempt to access admin endpoint as patient
curl -X GET https://localhost:5293/api/admin/users \
  -H "Authorization: Bearer <PATIENT_JWT_TOKEN>"
```

Expected response: `401 Unauthorized` or `403 Forbidden`

---

### Test 6: Token Refresh
**Objective**: Verify JWT refresh token mechanism

**Steps**:
1. Login and obtain tokens
2. Store access token and refresh token
3. Wait for access token to expire (if short-lived) or manually test:

**API Test**:
```bash
curl -X POST https://localhost:5293/api/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "<REFRESH_TOKEN>"
  }'
```

**Expected Results**:
- ✅ New access token issued
- ✅ Can use new token for subsequent requests
- ✅ Old token still works briefly (grace period)

---

### Test 7: Logout and Session Termination
**Objective**: Verify logout properly clears session

**Steps**:
1. Login as patient
2. Note JWT token from session/browser storage
3. Click "Logout" button
4. Try to access protected page (e.g., Dashboard)

**Expected Results**:
- ✅ Session cleared
- ✅ JWT token removed from browser storage
- ✅ Redirected to login page
- ✅ Cannot access protected pages

**API Verification** (using old token):
```bash
curl -X GET https://localhost:5293/api/patients/profile \
  -H "Authorization: Bearer <OLD_JWT_TOKEN>"
```

Expected response: `401 Unauthorized`

---

### Test 8: Invalid Credentials
**Objective**: Verify system rejects invalid credentials

**Steps**:
1. Navigate to login page
2. Enter:
   - Email: `patient1@healthcare.com`
   - Password: `WrongPassword123`
3. Click "Login"

**Expected Results**:
- ✅ Login fails
- ✅ Error message displayed: "Invalid email or password"
- ✅ Not redirected to dashboard
- ✅ Session not created

**API Verification**:
```bash
curl -X POST https://localhost:5293/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "patient1@healthcare.com",
    "password": "WrongPassword123"
  }'
```

Expected response: `401 Unauthorized`

---

### Test 9: Appointment Booking Flow (Patient)
**Objective**: Verify patient can book appointment (end-to-end)

**Steps**:
1. Login as patient (`patient1@healthcare.com`)
2. Navigate to "Book Appointment" or Appointments section
3. Select:
   - Doctor: "Dr. Aarav Mehta" (Cardiology)
   - Date: Future date (e.g., 5 days from now)
   - Time: 10:00 AM
   - Notes: "General checkup"
4. Click "Book Appointment"

**Expected Results**:
- ✅ Appointment created successfully
- ✅ Confirmation message displayed
- ✅ Appointment appears in patient's appointment list
- ✅ Appointment shows in doctor's appointment list

**API Verification**:
```bash
curl -X POST https://localhost:5293/api/appointments \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <PATIENT_JWT_TOKEN>" \
  -d '{
    "doctorId": 1,
    "appointmentDate": "2025-04-10T10:00:00Z",
    "notes": "General checkup"
  }'
```

---

### Test 10: Prescription Viewing (Patient)
**Objective**: Verify patient can view prescriptions from completed appointments

**Steps**:
1. Login as patient
2. Navigate to Prescriptions or Appointment Details
3. View prescription for completed appointment

**Expected Results**:
- ✅ Prescription details displayed (diagnosis, medicines, dosage)
- ✅ Medicines listed with instructions
- ✅ Can download or print prescription (if functionality exists)

---

### Test 11: Cross-Origin Requests (CORS)
**Objective**: Verify correct CORS headers in API responses

**Steps**:
1. Open browser console
2. Execute fetch from different origin:

```javascript
fetch('https://localhost:5293/api/doctors', {
  method: 'GET',
  credentials: 'include'
})
.then(r => r.json())
.then(d => console.log(d))
```

**Expected Results**:
- ✅ Request succeeds (CORS headers present)
- ✅ Doctor list returned
- ✅ Response headers include `Access-Control-Allow-Origin`

---

### Test 12: Error Handling
**Objective**: Verify proper error responses

**Test Invalid Endpoints**:
```bash
curl -X GET https://localhost:5293/api/invalid-endpoint
```

**Expected Results**:
- ✅ Returns `404 Not Found`
- ✅ Error response with clear message
- ✅ No stack traces exposed

**Test Invalid API Request**:
```bash
curl -X POST https://localhost:5293/api/appointments \
  -H "Content-Type: application/json" \
  -d '{"invalidField": "value"}'
```

**Expected Results**:
- ✅ Returns `400 Bad Request`
- ✅ Validation error messages provided
- ✅ No database errors exposed

---

## Performance Tests

### Test 13: Doctor List Performance
**Objective**: Verify patients can browse doctors efficiently

**Steps**:
1. Login as patient
2. Navigate to Doctor browsing section
3. Apply filters (specialization, fee range)
4. Verify response time under 1 second

**Expected Results**:
- ✅ Load time < 1 second for 5 doctors
- ✅ Filters work smoothly
- ✅ Doctor details display correctly

---

### Test 14: Appointment Pagination
**Objective**: Verify appointment lists handle pagination

**API Test**:
```bash
curl -X GET "https://localhost:5293/api/appointments?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <JWT_TOKEN>"
```

**Expected Results**:
- ✅ Returns paginated results
- ✅ Includes total count
- ✅ Correct page data

---

## Security Tests

### Test 15: SQL Injection Prevention
**Objective**: Verify system resists SQL injection

**Test**:
```bash
curl -X POST https://localhost:5293/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@healthcare.com\" OR \"1\"=\"1",
    "password": "anything"
  }'
```

**Expected Results**:
- ✅ Login fails
- ✅ No SQL errors exposed
- ✅ Parameterized queries in use

---

### Test 16: JWT Token Validation
**Objective**: Verify system validates JWT tokens properly

**Test with invalid token**:
```bash
curl -X GET https://localhost:5293/api/patients/profile \
  -H "Authorization: Bearer invalid.token.here"
```

**Expected Results**:
- ✅ Returns `401 Unauthorized`
- ✅ No token parsing errors exposed

---

## Success Criteria

✅ **Project is fully functional when**:
1. All 16 test cases pass
2. No unhandled exceptions
3. All CRUD operations for patients/doctors/appointments work
4. Authentication flows are seamless
5. Role-based access control enforced at all levels
6. Response times acceptable (< 1 second for normal operations)
7. Error messages clear and user-friendly
8. No sensitive data exposed in error messages

---

## Quick Test Checklist

- [ ] Patient Registration ✅
- [ ] Patient Login ✅
- [ ] Doctor Login ✅
- [ ] Admin Login ✅
- [ ] RBAC Working ✅
- [ ] Token Refresh ✅
- [ ] Logout ✅
- [ ] Invalid Credentials Rejected ✅
- [ ] Appointment Booking ✅
- [ ] Prescription Viewing ✅
- [ ] CORS Headers Present ✅
- [ ] Error Handling Proper ✅
- [ ] Performance Acceptable ✅
- [ ] SQL Injection Prevented ✅
- [ ] JWT Validation Working ✅

---

## Troubleshooting

### Login fails with "Invalid email or password"
- Verify user exists in database after seeding
- Check password is hashed correctly
- Ensure database migration was applied

### Token expires immediately
- Check JWT secret key configuration in `appsettings.json`
- Verify token expiry time is set correctly (default: 60 minutes)

### Can't access protected endpoints
- Verify JWT token is included in Authorization header: `Authorization: Bearer <token>`
- Check token format is correct (not expired)
- Verify Bearer scheme spelling

### CORS errors
- Verify MVC URL is in API's CORS allowed origins
- Check `Program.cs` CORS configuration
- Ensure credentials flag set correctly in fetch requests

---

## Logging and Monitoring

Monitor these logs while testing:
1. **API Logs**: `src/SmartHealthcare.API/Logs/` - Check for errors/warnings
2. **Browser Console**: JavaScript errors or network failures
3. **Network Tab**: Verify HTTP status codes are correct

---

## Next Steps After Testing

Once all tests pass:
1. Document any bugs found
2. Fix any failing tests
3. Perform user acceptance testing (UAT)
4. Deploy to staging/production environment
5. Monitor production logs for issues

