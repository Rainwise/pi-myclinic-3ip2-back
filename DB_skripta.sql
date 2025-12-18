CREATE DATABASE PI_Project
GO

USE PI_Project



CREATE TABLE [Admin] (
    IdUser INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
	PasswordSalt Nvarchar(max) not null;
    IsActive BIT NOT NULL DEFAULT 1
)

CREATE TABLE Specialization (
	IdSpecialization int primary key identity(1,1),
	[Name] nvarchar(50) NOT NULL
)

CREATE TABLE dbo.Doctors (
    IdDoctor INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NULL,
    SpecializationId INT foreign key references Specialization(IdSpecialization) NOT NULL,
    LicenseNumber NVARCHAR(50) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1
)

CREATE TABLE dbo.Patient (
    IdPatient INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NULL,
    IsActive BIT NOT NULL DEFAULT 1
)

CREATE TABLE HealthRecord (
	IdHealthRecord int primary key identity(1,1),
	PatientId int foreign key references Patient(IdPatient)
)

insert into Specialization values ('Opæa medicina')
insert into Specialization values ('Psihijatrija')
insert into Specialization values ('Plastièni kirurg')


select * from admin
select * from patient
select * from healthrecord
select * from Doctors
select * from Specialization