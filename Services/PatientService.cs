using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;

namespace myclinic_back.Services
{
    public class PatientService : IPatientService
    {
        private readonly PiProjectContext _context;

        public PatientService(PiProjectContext context)
        {
            _context = context;
        }

        public async Task<GetPatientDto> GetByIdAsync(int id)
        {
            var patient = await _context.Patients
                    .Include(p => p.HealthRecords)
                    .FirstOrDefaultAsync(p => p.IdPatient == id);

            GetPatientDto dto = new GetPatientDto()
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                IsActive = patient.IsActive,
                HealthRecordId = (int)patient.HealthRecords.FirstOrDefault(h => h.PatientId == id).PatientId,
            };

            return dto;
        }

        public async Task<List<GetPatientDto>> GetAllAsync()
        {
            var patients = await _context.Patients
                   .Include(p => p.HealthRecords)
                   .ToListAsync();

            var dtos = new List<GetPatientDto>();

            foreach (var p in patients)
            {
                GetPatientDto dto = new GetPatientDto()
                {
                    IdPatient = p.IdPatient,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    IsActive = p.IsActive,
                    HealthRecordId = (int)p.HealthRecords.FirstOrDefault(h => h.PatientId == p.IdPatient).PatientId,
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task CreateObjectAsync(PatientDto dto)
        {
            var patient = new Patient()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive,
            };

            _context.Add(patient);
            _context.SaveChanges();

            var healthRecord = new HealthRecord()
            {
                PatientId = patient.IdPatient
            };

            _context.Add(healthRecord);
            _context.SaveChanges();

        }

        public async Task UpdateObjectAsync(int id, PatientDto dto)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);

            patient.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) || dto.FirstName == "string" ? patient.FirstName : dto.FirstName;
            patient.LastName = string.IsNullOrWhiteSpace(dto.LastName) || dto.LastName == "string" ? patient.LastName : dto.LastName;
            patient.Email = string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "string" ? patient.Email : dto.Email;
            patient.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.PhoneNumber == "string" ? patient.PhoneNumber : dto.PhoneNumber;
            patient.IsActive = dto.IsActive;

            _context.Update(patient);
            _context.SaveChanges();

        }

        public async Task DeleteObjectAsync(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.IdPatient == id);

            var healthRecord = _context.HealthRecords.FirstOrDefault(h => h.PatientId == id);

            _context.HealthRecords.Remove(healthRecord);
            _context.Remove(patient);
            _context.SaveChanges();
        }
    }
}
