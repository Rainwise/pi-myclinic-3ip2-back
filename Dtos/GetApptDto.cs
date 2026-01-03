using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Dtos
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetApptDto : ControllerBase
    {
        public int IdAppointment { get; set; }

        public string Doctor { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime StartsAt { get; set; }

    }
}
