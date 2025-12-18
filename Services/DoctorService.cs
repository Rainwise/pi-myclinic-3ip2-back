using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;

namespace myclinic_back.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly PiProjectContext _context;

        public DoctorService(PiProjectContext context)
        {
            _context = context;
        }

        public async Task<GetDoctorDto> GetByIdAsync(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.IdDoctor == id);

            GetDoctorDto dto = new GetDoctorDto()
            {
                IdDoctor = doctor.IdDoctor,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization.Name,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                LicenseNumber = doctor.LicenseNumber,
                IsActive = doctor.IsActive,
            };

            return dto;
        }

        public async Task<List<GetDoctorDto>> GetAllAsync()
        {
            var doctors = await _context.Doctors
                    .Include(d => d.Specialization)
                    .ToListAsync();

            var dtos = new List<GetDoctorDto>();

            foreach (var d in doctors)
            {
                GetDoctorDto dto = new GetDoctorDto()
                {
                    IdDoctor = d.IdDoctor,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Specialization = d.Specialization.Name,
                    Email = d.Email,
                    PhoneNumber = d.PhoneNumber,
                    LicenseNumber = d.LicenseNumber,
                    IsActive = d.IsActive,
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task CreateObjectAsync(DoctorDto dto)
        {
            var doctor = new Doctor()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                LicenseNumber = dto.LicenseNumber,
                IsActive = dto.IsActive,
                SpecializationId = dto.SpecializationId,
            };

            _context.Add(doctor);
            _context.SaveChanges();

        }

        public async Task UpdateObjectAsync(int id, DoctorDto dto)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.IdDoctor == id);
                

            doctor.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) || dto.FirstName == "string" ? doctor.FirstName : dto.FirstName;
            doctor.LastName = string.IsNullOrWhiteSpace(dto.LastName) || dto.LastName == "string" ? doctor.LastName : dto.LastName;
            doctor.Email = string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "string" ? doctor.Email : dto.Email;
            doctor.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.PhoneNumber == "string" ? doctor.PhoneNumber : dto.PhoneNumber;
            doctor.LicenseNumber = string.IsNullOrWhiteSpace(dto.LicenseNumber) || dto.LicenseNumber == "string" ? doctor.LicenseNumber : dto.LicenseNumber;
            doctor.IsActive = dto.IsActive;
            doctor.SpecializationId = string.IsNullOrWhiteSpace(_context.Specializations.FirstOrDefault(s => s.IdSpecialization == dto.SpecializationId).Name) || dto.SpecializationId == 0 ? doctor.SpecializationId : dto.SpecializationId;

            _context.Update(doctor);
            _context.SaveChanges();

        }

        public async Task DeleteObjectAsync(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == id);

            _context.Remove(doctor);
            _context.SaveChanges();

        }
    }
}
