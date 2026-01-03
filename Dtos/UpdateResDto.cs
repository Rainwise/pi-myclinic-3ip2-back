using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Dtos
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateResDto : ControllerBase
    {
        public int IdReservation { get; set; }
        public int? AppointmentId { get; set; }

        public int? PatientId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
