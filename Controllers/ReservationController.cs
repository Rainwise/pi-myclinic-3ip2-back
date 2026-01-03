using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.Dtos;
using myclinic_back.Models;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly PiProjectContext _context;
        private readonly IConfiguration _configuration;

        public ReservationController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{idReservation}")]
        public ActionResult GetReservationById(int idReservation)
        {
            try
            {
                var res = _context.Reservations.FirstOrDefault(r => r.IdReservation == idReservation);

                if (res == null)
                {
                    return NotFound();
                }

                GetResDto dto = new GetResDto()
                {
                    IdReservation = res.IdReservation,
                    PatientId = res.PatientId,
                    CreatedAt = res.CreatedAt,
                    AppointmentId = res.AppointmentId,
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]/{idPatient}")]
        public ActionResult GetReservationsByIdPatient(int idPatient)
        {
            try
            {
                var ress = _context.Reservations
                        .Include(a => a.Appointment)
                        .Include(a => a.Patient.Account)
                        .Where(a => a.PatientId == idPatient)
                        .ToList();

                var dtos = new List<GetResDto>();

                foreach (var r in ress)
                {
                    var dto = new GetResDto()
                    {
                        IdReservation = r.IdReservation,
                        PatientId = r.PatientId,
                        CreatedAt = r.CreatedAt,
                        AppointmentId = r.AppointmentId
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
        public ActionResult GetReservations()
        {
            try
            {
                var ress = _context.Reservations.ToList();

                var dtos = new List<GetResDto>();

                foreach (var r in ress)
                {
                    var dto = new GetResDto()
                    {
                        IdReservation = r.IdReservation,
                        PatientId = r.PatientId,
                        CreatedAt = r.CreatedAt,
                        AppointmentId = r.AppointmentId
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
        public ActionResult CreateReservation(int idDoctor, CreateResDto dto)
        {
            try
            {
                var res = new Reservation()
                {
                    PatientId = dto.PatientId,
                    CreatedAt = dto.CreatedAt,
                    AppointmentId = dto.AppointmentId
                };

                _context.Reservations.Add(res);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("[action]")]
        public ActionResult DeleteReservation(int idReservation)
        {
            try
            {
                var res = _context.Reservations.FirstOrDefault(a => a.IdReservation == idReservation);

                if (res == null)
                {
                    return NotFound();
                }

                _context.Remove(res);
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
