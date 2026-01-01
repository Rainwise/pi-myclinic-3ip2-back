using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using System.Numerics;

namespace myclinic_back.Services
{
    public class PatientService : IPatientService
    {
        private readonly PiProjectContext _context;
        private readonly ICrudLogger _logger;
        public PatientService(PiProjectContext context, ICrudLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetPatientDto> GetByIdAsync(int id)
        {
            var patient = await _context.Patients
                    .Include(p => p.HealthRecords)
                    .FirstOrDefaultAsync(p => p.IdPatient == id);

            var dto = GetObject(patient);
            
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
                var dto = GetObject(p);

                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task CreateObjectAsync(PatientDto dto)
        {
           
            var patient = CreateObject(dto);
            _context.Add(patient);
            _context.SaveChanges();

            var healthRecord = CreateHealthRecord(patient.IdPatient);
            _context.Add(healthRecord);
            _context.SaveChanges();

            _logger.LogCrudAsync("Patient", "Created", patient.IdPatient);

        }

        public async Task UpdateObjectAsync(int id, PatientDto dto)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);

            _context.Update(UpdateObject(patient, dto));
            _context.SaveChanges();

            _logger.LogCrudAsync("Patient", "Updated", patient.IdPatient);

        }

        public async Task DeleteObjectAsync(int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);

            var healthRecord = await _context.HealthRecords.FirstOrDefaultAsync(h => h.PatientId == id);

            _context.HealthRecords.Remove(healthRecord);
            _context.Remove(patient);
            _context.SaveChanges();

            _logger.LogCrudAsync("Patient", "Deleted", patient.IdPatient);
        }

        public GetPatientDto GetObject(Patient patient)
        {
            GetPatientDto dto = new GetPatientDto()
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                IsActive = patient.IsActive,
                HealthRecordId = (int)patient.HealthRecords.FirstOrDefault(h => h.PatientId == patient.IdPatient).PatientId,
            };

            return dto;
        }

        public Patient CreateObject(PatientDto dto)
        {
            var patient = new Patient()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive,
            };

           
            return patient;
        }

        public HealthRecord CreateHealthRecord(int patientId)
        {
            var healthRecord = new HealthRecord()
            {
                PatientId = patientId
            };

            return healthRecord;
        }

        public Patient UpdateObject(Patient patient, PatientDto dto)
        {
            patient.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) || dto.FirstName == "string" ? patient.FirstName : dto.FirstName;
            patient.LastName = string.IsNullOrWhiteSpace(dto.LastName) || dto.LastName == "string" ? patient.LastName : dto.LastName;
            patient.Email = string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "string" ? patient.Email : dto.Email;
            patient.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.PhoneNumber == "string" ? patient.PhoneNumber : dto.PhoneNumber;
            patient.IsActive = dto.IsActive;

            return patient;
        }
    }
}
