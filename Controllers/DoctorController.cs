using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Models;
using System.Numerics;

namespace myclinic_back.Controllers
{
    [Route("api/Doctors")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public DoctorController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("{idDoctor}")]
        public ActionResult GetDoctorById(int idDoctor)
        {
            try
            {
                var doctor = _context.Doctors
                    .Include(d => d.Specialization)
                    .FirstOrDefault(d => d.IdDoctor == idDoctor);

                if (doctor == null)
                {
                    return NotFound();
                }

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

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetDoctors()
        {
            try
            {
                var doctors = _context.Doctors
                    .Include(d => d.Specialization)
                    .ToList();

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

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateDoctor(CreateDoctorDto dto)
        {
            try
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

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{idDoctor}")]
        public ActionResult UpdateDoctor(int idDoctor, UpdateDoctorDto dto)
        {
            try
            {
                var doctor = _context.Doctors.FirstOrDefault(d => d.IdDoctor == idDoctor);

                if (doctor == null)
                {
                    return NotFound();
                }

                doctor.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) || dto.FirstName == "string" ? doctor.FirstName : dto.FirstName;
                doctor.LastName = string.IsNullOrWhiteSpace(dto.LastName) || dto.LastName == "string" ? doctor.LastName : dto.LastName;
                doctor.Email = string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "string" ? doctor.Email : dto.Email;
                doctor.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.PhoneNumber == "string" ? doctor.PhoneNumber : dto.PhoneNumber;
                doctor.LicenseNumber = string.IsNullOrWhiteSpace(dto.LicenseNumber) || dto.LicenseNumber == "string" ? doctor.LicenseNumber : dto.LicenseNumber;
                doctor.IsActive = dto.IsActive;
                doctor.SpecializationId = string.IsNullOrWhiteSpace(_context.Specializations.FirstOrDefault(s => s.IdSpecialization == dto.SpecializationId).Name) || dto.SpecializationId == 0 ? doctor.SpecializationId : dto.SpecializationId;

                _context.Update(doctor);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{idDoctor}")]
        public ActionResult DeleteDoctor(int idDoctor)
        {
            try
            {
                var doctor = _context.Doctors.FirstOrDefault(d => d.IdDoctor == idDoctor);

                if (doctor == null)
                {
                    return NotFound();
                }

                _context.Remove(doctor);
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
