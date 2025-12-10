
CREATE DATABASE ClinicDB;
GO
USE ClinicDB;
GO


-- Users
CREATE TABLE dbo.Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Admin',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Doctors
CREATE TABLE dbo.Doctors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NULL,
    Specialization NVARCHAR(100) NOT NULL,
    LicenseNumber NVARCHAR(50) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
);
GO


-- Seed doctor
INSERT INTO dbo.Doctors (FirstName, LastName, Email, PhoneNumber, Specialization, LicenseNumber, DateOfBirth, HireDate)
VALUES
    ('John', 'Smith', 'john.smith@clinic.com', '+1-555-0101', 'Cardiology', 'LIC-12345', '1980-05-15', '2020-01-15'),
    ('Sarah', 'Johnson', 'sarah.johnson@clinic.com', '+1-555-0102', 'Pediatrics', 'LIC-12346', '1985-08-22', '2021-03-10'),
    ('Michael', 'Brown', 'michael.brown@clinic.com', '+1-555-0103', 'Orthopedics', 'LIC-12347', '1978-11-30', '2019-06-01');
GO
