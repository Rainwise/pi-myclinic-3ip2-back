using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.Dtos;
using myclinic_back.Models;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public AppointmentController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{idAppointment}")]
        public ActionResult GetAppointmentById(int idAppointment)
        {
            try
            {
                var appt = _context.Appointments.FirstOrDefault(a => a.IdAppointment == idAppointment);

                if (appt == null)
                {
                    return NotFound();
                }

                GetApptDto dto = new GetApptDto()
                {
                    IdAppointment = appt.IdAppointment
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]/{idDoctor}")]
        public ActionResult GetAppointmentsByIdDoctor(int idDoctor)
        {
            try
            {
                var appt = _context.Appointments
                        .Include(a => a.Doctor)
                        .Include(a => a.Doctor.Account)
                        .Where(a => a.DoctorId == idDoctor)
                        .ToList();

                var dtos = new List<GetApptDto>();

                foreach (var a in appt)
                {
                    var dto = new GetApptDto()
                    {
                        IdAppointment = a.IdAppointment,
                        Doctor = a.Doctor.Account.FirstName + " " + a.Doctor.Account.LastName,
                        StartsAt = a.StartsAt,
                        CreatedAt = a.CreatedAt,
                    };
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetAppointments()
        {
            try
            {
                var appt = _context.Appointments.ToList();

                var dtos = new List<GetApptDto>();

                foreach (var a in appt)
                {
                    var dto = new GetApptDto()
                    {
                        IdAppointment = a.IdAppointment,
                        Doctor = a.Doctor.Account.FirstName + " " + a.Doctor.Account.LastName,
                        StartsAt = a.StartsAt,
                        CreatedAt = a.CreatedAt,
                    };
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CreateAppointment(int idDoctor, CreateApptDto dto)
        {
            try
            {
                var Appointment = new Appointment()
                {
                    DoctorId = idDoctor,
                    StartsAt = dto.StartsAt,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Appointments.Add(Appointment);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult UpdateAppointment(UpdateApptDto dto)
        {
            try
            {
                var appt = _context.Appointments
                        .Include(a => a.Doctor)
                        .FirstOrDefault(a => a.IdAppointment == dto.IdAppointment);

                appt.DoctorId = string.IsNullOrWhiteSpace(_context.Doctors.FirstOrDefault(d => d.AccountId == dto.DoctorId).Account.LastName) ? appt.DoctorId : dto.DoctorId;
                appt.StartsAt = dto.StartsAt;

                _context.Update(appt);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("[action]")]
        public ActionResult DeleteAppointment(int idAppointment)
        {
            try
            {
                var appt = _context.Appointments.FirstOrDefault(a => a.IdAppointment == idAppointment);

                if (appt == null)
                {
                    return NotFound();
                }

                _context.Remove(appt);
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
