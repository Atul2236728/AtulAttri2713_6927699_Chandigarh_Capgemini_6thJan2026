-- =============================================
-- Smart Hospital Management System
-- Database Setup Script
-- SQL Server
-- =============================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'HospitalDB')
    DROP DATABASE HospitalDB;
GO

CREATE DATABASE HospitalDB;
GO

USE HospitalDB;
GO

-- =============================================
-- USERS TABLE
-- =============================================
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(15),
    Role NVARCHAR(20) CHECK (Role IN ('Admin','Doctor','Patient')) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- DEPARTMENTS TABLE
-- =============================================
CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    IconClass NVARCHAR(50) DEFAULT 'fa-stethoscope',
    IsActive BIT DEFAULT 1
);
GO

-- =============================================
-- DOCTORS TABLE
-- =============================================
CREATE TABLE Doctors (
    DoctorId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT UNIQUE NOT NULL,
    DepartmentId INT NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    ExperienceYears INT DEFAULT 0,
    Availability NVARCHAR(200),
    ConsultationFee DECIMAL(10,2) NOT NULL DEFAULT 500.00,
    Qualification NVARCHAR(200),
    ProfileImage NVARCHAR(255) DEFAULT 'default-doctor.png',
    IsAvailable BIT DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);
GO

-- =============================================
-- PATIENT PROFILES TABLE
-- =============================================
CREATE TABLE PatientProfiles (
    ProfileId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT UNIQUE NOT NULL,
    DateOfBirth DATE,
    Gender NVARCHAR(10) CHECK (Gender IN ('Male','Female','Other')),
    BloodGroup NVARCHAR(5),
    Address NVARCHAR(500),
    EmergencyContact NVARCHAR(15),
    MedicalHistory NVARCHAR(MAX),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);
GO

-- =============================================
-- APPOINTMENTS TABLE
-- =============================================
CREATE TABLE Appointments (
    AppointmentId INT PRIMARY KEY IDENTITY(1,1),
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    TimeSlot NVARCHAR(20),
    Reason NVARCHAR(500),
    Status NVARCHAR(20) CHECK (Status IN ('Booked','Confirmed','Completed','Cancelled')) DEFAULT 'Booked',
    CreatedAt DATETIME DEFAULT GETDATE(),
    Notes NVARCHAR(500),
    FOREIGN KEY (PatientId) REFERENCES Users(UserId),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
);
GO

-- =============================================
-- PRESCRIPTIONS TABLE (One-to-One with Appointment)
-- =============================================
CREATE TABLE Prescriptions (
    PrescriptionId INT PRIMARY KEY IDENTITY(1,1),
    AppointmentId INT UNIQUE NOT NULL,
    Diagnosis NVARCHAR(500) NOT NULL,
    Medicines NVARCHAR(MAX),
    Notes NVARCHAR(500),
    FollowUpDate DATE,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(AppointmentId) ON DELETE CASCADE
);
GO

-- =============================================
-- MEDICINES TABLE
-- =============================================
CREATE TABLE Medicines (
    MedicineId INT PRIMARY KEY IDENTITY(1,1),
    MedicineName NVARCHAR(100) NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    Unit NVARCHAR(30) DEFAULT 'Tablet',
    IsActive BIT DEFAULT 1
);
GO

-- =============================================
-- PRESCRIPTION MEDICINES (Many-to-Many)
-- =============================================
CREATE TABLE PrescriptionMedicines (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PrescriptionId INT NOT NULL,
    MedicineId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    Dosage NVARCHAR(100),
    Duration NVARCHAR(50),
    TotalPrice AS (CAST(Quantity AS DECIMAL(10,2)) * (SELECT UnitPrice FROM Medicines WHERE MedicineId = PrescriptionMedicines.MedicineId)),
    FOREIGN KEY (PrescriptionId) REFERENCES Prescriptions(PrescriptionId) ON DELETE CASCADE,
    FOREIGN KEY (MedicineId) REFERENCES Medicines(MedicineId)
);
GO

-- =============================================
-- BILLS TABLE
-- =============================================
CREATE TABLE Bills (
    BillId INT PRIMARY KEY IDENTITY(1,1),
    AppointmentId INT UNIQUE NOT NULL,
    ConsultationFee DECIMAL(10,2) NOT NULL,
    MedicineCharges DECIMAL(10,2) DEFAULT 0,
    OtherCharges DECIMAL(10,2) DEFAULT 0,
    Discount DECIMAL(10,2) DEFAULT 0,
    TotalAmount AS (ConsultationFee + MedicineCharges + OtherCharges - Discount) PERSISTED,
    PaymentStatus NVARCHAR(20) CHECK (PaymentStatus IN ('Paid','Unpaid','Partial')) DEFAULT 'Unpaid',
    PaymentMethod NVARCHAR(30),
    PaidAt DATETIME,
    GeneratedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(AppointmentId)
);
GO

PRINT 'All tables created successfully.';
GO
