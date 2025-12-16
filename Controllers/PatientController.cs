using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Models;
using System.Numerics;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public PatientController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("{idPatient}")]
        public ActionResult GetPatientById(int idPatient)
        {
            try
            {
                var patient = _context.Patients
                    .Include(p => p.HealthRecords)
                    .FirstOrDefault(p => p.IdPatient == idPatient);

                if (patient == null)
                {
                    return NotFound();
                }

                GetPatientDto dto = new GetPatientDto()
                {
                    IdPatient = patient.IdPatient,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Email = patient.Email,
                    PhoneNumber = patient.PhoneNumber,
                    IsActive = patient.IsActive,
                    HealthRecordId = (int)patient.HealthRecords.FirstOrDefault(h => h.PatientId == idPatient).PatientId,
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetPatients()
        {
            try
            {
                var patients = _context.Patients
                    .Include(p => p.HealthRecords)
                    .ToList();

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

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CreatePatient(CreatePatientDto dto)
        {
            try
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

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{idPatient}")]
        public ActionResult UpdatePatient(int idPatient, UpdatePatientDto dto)
        {
            try
            {
                var patient = _context.Patients.FirstOrDefault(p => p.IdPatient == idPatient);

                if (patient == null)
                {
                    return NotFound();
                }

                patient.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) || dto.FirstName == "string" ? patient.FirstName : dto.FirstName;
                patient.LastName = string.IsNullOrWhiteSpace(dto.LastName) || dto.LastName == "string" ? patient.LastName : dto.LastName;
                patient.Email = string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "string" ? patient.Email : dto.Email;
                patient.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.PhoneNumber == "string" ? patient.PhoneNumber : dto.PhoneNumber;
                patient.IsActive = dto.IsActive;
               
                _context.Update(patient);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{idPatient}")]
        public ActionResult DeletePatient(int idPatient)
        {
            try
            {
                var patient = _context.Patients.FirstOrDefault(p => p.IdPatient == idPatient);

                if (patient == null)
                {
                    return NotFound();
                }

                var healthRecord = _context.HealthRecords.FirstOrDefault(h => h.PatientId == idPatient);

                _context.HealthRecords.Remove(healthRecord);
                _context.Remove(patient);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
