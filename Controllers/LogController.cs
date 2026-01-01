using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myclinic_back.DTOs;
using myclinic_back.Models;

namespace myclinic_back.Controllers
{
    [Route("api/Logs")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {
        private readonly PiProjectContext _context;

        public LogController(PiProjectContext context) 
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult GetLogById(int id)
        {
            var log = _context.Logs.FirstOrDefault(l => l.IdLog == id);

            var dto = new GetLogDto()
            {
                Message = log.Message,
                Timestamp = log.Timestamp,
            };

            return Ok(dto);
        }

        [HttpGet]
        public ActionResult GetLogs()
        {
            var logs = _context.Logs.ToList();

            var dtos = new List<GetLogDto>();

            foreach (var l in logs)
            {
                var dto = new GetLogDto()
                {
                    Message = l.Message,
                    Timestamp = l.Timestamp
                };

                dtos.Add(dto);
            }

            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult CreateLog(LogDto dto)
        {
            Log log = new Log()
            {
                Message = dto.Message,
                Timestamp = DateTime.UtcNow.AddHours(1)
            };

            _context.Add(log);
            _context.SaveChanges();

            return Ok();
        }

    }
}
