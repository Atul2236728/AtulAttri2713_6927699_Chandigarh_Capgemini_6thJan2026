-- =============================================
-- Smart Hospital Management System
-- Seed Data Script
-- Run AFTER 01_CreateDatabase.sql
-- =============================================

USE HospitalDB;
GO

-- =============================================
-- DEPARTMENTS
-- =============================================
INSERT INTO Departments (DepartmentName, Description, IconClass) VALUES
('Cardiology',       'Heart and cardiovascular system specialists',          'fa-heartbeat'),
('Neurology',        'Brain, spinal cord and nervous system specialists',    'fa-brain'),
('Orthopedics',      'Bones, joints and musculoskeletal system',            'fa-bone'),
('Pediatrics',       'Medical care for infants, children and adolescents',  'fa-baby'),
('Dermatology',      'Skin, hair, and nail conditions',                     'fa-hand-sparkles'),
('Ophthalmology',    'Eyes and vision care',                                'fa-eye'),
('Gynecology',       'Female reproductive health',                          'fa-venus'),
('Dentistry',        'Oral health, teeth and gums',                         'fa-tooth'),
('General Medicine', 'Primary healthcare and general ailments',             'fa-user-md'),
('Psychiatry',       'Mental health and behavioral disorders',              'fa-head-side-virus');
GO

-- =============================================
-- USERS (Password: "Test@1234" → bcrypt hash stored as placeholder)
-- In production these would be real BCrypt hashes. For dev use:
-- Admin: admin@hospital.com / Admin@123
-- Doctors: doctor1@hospital.com / Doctor@123
-- Patient: patient1@hospital.com / Patient@123
-- NOTE: The API generates proper BCrypt hashes on register. 
-- These are SHA256 placeholders for seed data.
-- =============================================
-- Admin User
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES
('Super Admin', 'admin@hospital.com',
 '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', -- Admin@123
 '9876543210', 'Admin');

-- Doctor Users
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES
('Dr. Rajesh Sharma',   'rajesh.sharma@hospital.com',   '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345671', 'Doctor'),
('Dr. Priya Mehta',     'priya.mehta@hospital.com',     '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345672', 'Doctor'),
('Dr. Anil Kumar',      'anil.kumar@hospital.com',      '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345673', 'Doctor'),
('Dr. Sunita Patel',    'sunita.patel@hospital.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345674', 'Doctor'),
('Dr. Vikram Singh',    'vikram.singh@hospital.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345675', 'Doctor'),
('Dr. Meena Iyer',      'meena.iyer@hospital.com',      '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345676', 'Doctor'),
('Dr. Arjun Kapoor',    'arjun.kapoor@hospital.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345677', 'Doctor'),
('Dr. Kavita Reddy',    'kavita.reddy@hospital.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345678', 'Doctor'),
('Dr. Suresh Nair',     'suresh.nair@hospital.com',     '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345679', 'Doctor'),
('Dr. Anjali Gupta',    'anjali.gupta@hospital.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9812345680', 'Doctor');

-- Patient Users
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Role) VALUES
('Rahul Verma',     'rahul.verma@gmail.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9900001111', 'Patient'),
('Pooja Singh',     'pooja.singh@gmail.com',    '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9900002222', 'Patient'),
('Amit Joshi',      'amit.joshi@gmail.com',     '$2a$11$rBnvnGbRs7yQtB.x5TJnKeYS9mFb3E6E0q0Wm/uD1cN5tVOKtmCuS', '9900003333', 'Patient');
GO

-- =============================================
-- DOCTORS PROFILES
-- =============================================
INSERT INTO Doctors (UserId, DepartmentId, Specialization, ExperienceYears, Availability, ConsultationFee, Qualification) VALUES
(2, 1, 'Interventional Cardiologist',    15, 'Mon-Fri 9AM-5PM',  1500.00, 'MBBS, MD (Cardiology), DM'),
(3, 2, 'Neurologist',                    12, 'Mon-Sat 10AM-4PM', 1200.00, 'MBBS, MD (Neurology), DM'),
(4, 3, 'Orthopedic Surgeon',             10, 'Tue-Sat 9AM-3PM',  1000.00, 'MBBS, MS (Orthopedics)'),
(5, 4, 'Pediatrician',                    8, 'Mon-Fri 8AM-2PM',   800.00, 'MBBS, MD (Pediatrics)'),
(6, 5, 'Dermatologist & Cosmetologist',   7, 'Mon-Sat 11AM-6PM',  900.00, 'MBBS, MD (Dermatology)'),
(7, 6, 'Ophthalmologist',                11, 'Mon-Fri 9AM-5PM',  1100.00, 'MBBS, MS (Ophthalmology)'),
(8, 7, 'Gynecologist & Obstetrician',    14, 'Mon-Sat 10AM-5PM', 1300.00, 'MBBS, MD (Gynecology), DGO'),
(9, 8, 'Dental Surgeon',                  6, 'Mon-Sat 10AM-7PM',  700.00, 'BDS, MDS'),
(10,9, 'General Physician',               9, 'Mon-Sun 8AM-8PM',   600.00, 'MBBS, MD'),
(11,10,'Psychiatrist',                   13, 'Mon-Fri 2PM-8PM',  1400.00, 'MBBS, MD (Psychiatry), DPM');
GO

-- =============================================
-- PATIENT PROFILES
-- =============================================
INSERT INTO PatientProfiles (UserId, DateOfBirth, Gender, BloodGroup, Address) VALUES
(12, '1990-05-15', 'Male',   'O+', 'House 12, Sector 7, Chandigarh'),
(13, '1995-08-22', 'Female', 'A+', 'Flat 5, Green Valley, Mohali'),
(14, '1988-11-30', 'Male',   'B+', '23, Civil Lines, Ludhiana');
GO

-- =============================================
-- MEDICINES
-- =============================================
INSERT INTO Medicines (MedicineName, UnitPrice, Unit) VALUES
('Paracetamol 500mg',       5.00,  'Tablet'),
('Amoxicillin 500mg',      12.00,  'Capsule'),
('Ibuprofen 400mg',         8.00,  'Tablet'),
('Pantoprazole 40mg',      15.00,  'Tablet'),
('Cetirizine 10mg',         6.00,  'Tablet'),
('Metformin 500mg',        10.00,  'Tablet'),
('Atorvastatin 10mg',      25.00,  'Tablet'),
('Amlodipine 5mg',         18.00,  'Tablet'),
('Azithromycin 500mg',     35.00,  'Tablet'),
('Vitamin D3 60000IU',     45.00,  'Capsule'),
('Calcium Carbonate 500mg', 8.00,  'Tablet'),
('Omeprazole 20mg',        12.00,  'Capsule'),
('Montelukast 10mg',       22.00,  'Tablet'),
('Salbutamol Inhaler',    180.00,  'Piece'),
('B-Complex Syrup 200ml',  95.00,  'Piece');
GO

-- =============================================
-- SAMPLE APPOINTMENTS
-- =============================================
INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, TimeSlot, Reason, Status) VALUES
(12, 1, DATEADD(DAY, -5, GETDATE()), '10:00 AM', 'Chest pain and shortness of breath', 'Completed'),
(12, 3, DATEADD(DAY,  2, GETDATE()), '11:00 AM', 'Knee pain after sports injury',       'Booked'),
(13, 5, DATEADD(DAY, -2, GETDATE()), '02:00 PM', 'Skin rash and itching',               'Completed'),
(14, 9, DATEADD(DAY,  1, GETDATE()), '09:00 AM', 'General checkup',                     'Confirmed');
GO

-- =============================================
-- SAMPLE PRESCRIPTIONS
-- =============================================
INSERT INTO Prescriptions (AppointmentId, Diagnosis, Medicines, Notes, FollowUpDate) VALUES
(1, 'Stable Angina', 'Aspirin 75mg daily, Atorvastatin 10mg at night, Amlodipine 5mg morning',
   'Avoid strenuous activity. Monitor BP daily. Low-salt diet.', DATEADD(DAY, 25, GETDATE())),
(3, 'Contact Dermatitis', 'Cetirizine 10mg once daily, Calamine lotion topical twice daily',
   'Avoid the allergen. Do not scratch. Keep area clean and dry.', DATEADD(DAY, 14, GETDATE()));
GO

-- =============================================
-- SAMPLE BILLS
-- =============================================
INSERT INTO Bills (AppointmentId, ConsultationFee, MedicineCharges, OtherCharges, PaymentStatus, PaymentMethod, PaidAt) VALUES
(1, 1500.00, 250.00, 100.00, 'Paid',   'Cash',    DATEADD(DAY, -5, GETDATE())),
(3,  900.00, 180.00,   0.00, 'Unpaid', NULL,      NULL);
GO

PRINT 'Seed data inserted successfully.';
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- Get Doctor with full info
CREATE PROCEDURE sp_GetDoctorById @DoctorId INT
AS
BEGIN
    SELECT d.*, u.FullName, u.Email, u.Phone, dep.DepartmentName, dep.Description AS DeptDescription
    FROM Doctors d
    JOIN Users u ON d.UserId = u.UserId
    JOIN Departments dep ON d.DepartmentId = dep.DepartmentId
    WHERE d.DoctorId = @DoctorId AND u.IsActive = 1;
END
GO

-- Get Appointments with full details
CREATE PROCEDURE sp_GetAppointmentsByDoctor @DoctorId INT
AS
BEGIN
    SELECT a.*, 
           pu.FullName AS PatientName, pu.Email AS PatientEmail, pu.Phone AS PatientPhone,
           du.FullName AS DoctorName,
           p.Diagnosis, p.Medicines AS PrescriptionMedicines, p.Notes AS PrescriptionNotes,
           b.TotalAmount, b.PaymentStatus
    FROM Appointments a
    JOIN Users pu ON a.PatientId = pu.UserId
    JOIN Doctors d ON a.DoctorId = d.DoctorId
    JOIN Users du ON d.UserId = du.UserId
    LEFT JOIN Prescriptions p ON a.AppointmentId = p.AppointmentId
    LEFT JOIN Bills b ON a.AppointmentId = b.AppointmentId
    WHERE a.DoctorId = @DoctorId
    ORDER BY a.AppointmentDate DESC;
END
GO

PRINT 'Stored procedures created successfully.';
GO
