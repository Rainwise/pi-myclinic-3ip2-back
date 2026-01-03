//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using myclinic_back.Dtos;
//using myclinic_back.Models;

//namespace myclinic_back.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NotificationController : ControllerBase
//    {
//        private readonly PiProjectContext _context;
//        private readonly IConfiguration _configuration;

//        public NotificationController(PiProjectContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        [HttpGet("[action]/{idNotification}")]
//        public ActionResult GetNotificationByIdNotification(int idNotification)
//        {
//            try
//            {
//                var n = _context.Notifications.FirstOrDefault(r => r.IdNotification == idNotification);

//                if (n == null)
//                {
//                    return NotFound();
//                }

//                GetNotifDto dto = new GetNotifDto()
//                {
//                    IdNotification = n.IdNotification,
//                    ReservationId =  n.ReservationId
//                };

//                return Ok(dto);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }

//        [HttpGet("[action]/{idAccount}")]
//        public ActionResult GetNotificationByAccountId(int idAccount)
//        {
//            try
//            {
//                var account = _context.Accounts.FirstOrDefault(a => a.IdAccount == idAccount);

//                var 
//                var ress = _context.Notifications
//                        .Include(n => n.Reservation)
//                        .Include
//                        .Where(a => a.IdNotification == idAccount)
//                        .ToList();

//                var dtos = new List<GetResDto>();

//                foreach (var r in ress)
//                {
//                    var dto = new GetResDto()
//                    {
//                        IdReservation = r.IdReservation,
//                        PatientId = r.PatientId,
//                        CreatedAt = r.CreatedAt,
//                        AppointmentId = r.AppointmentId
//                    };
//                }

//                return Ok(dtos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }

//        [HttpGet("[action]")]
//        public ActionResult GetReservations()
//        {
//            try
//            {
//                var ress = _context.Reservations.ToList();

//                var dtos = new List<GetResDto>();

//                foreach (var r in ress)
//                {
//                    var dto = new GetResDto()
//                    {
//                        IdReservation = r.IdReservation,
//                        PatientId = r.PatientId,
//                        CreatedAt = r.CreatedAt,
//                        AppointmentId = r.AppointmentId
//                    };
//                }

//                return Ok(dtos);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }

//        [HttpPost("[action]")]
//        public ActionResult CreateReservation(int idDoctor, CreateResDto dto)
//        {
//            try
//            {
//                var res = new Reservation()
//                {
//                    PatientId = dto.PatientId,
//                    CreatedAt = dto.CreatedAt,
//                    AppointmentId = dto.AppointmentId
//                };

//                _context.Reservations.Add(res);
//                _context.SaveChanges();

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }

//        [HttpDelete("[action]")]
//        public ActionResult DeleteReservation(int idReservation)
//        {
//            try
//            {
//                var res = _context.Reservations.FirstOrDefault(a => a.IdReservation == idReservation);

//                if (res == null)
//                {
//                    return NotFound();
//                }

//                _context.Remove(res);
//                _context.SaveChanges();

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }
//    }
//}
