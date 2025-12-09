using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myclinic_back.Dtos;
using myclinic_back.Models;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialisationController : ControllerBase
    {
        public PiProjectContext _context;
        public IConfiguration _configuration;

        public SpecialisationController(PiProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{idSpecialisation}")]
        public ActionResult GetSpecialisationById(int idSpecialisation)
        {
            try
            {
                var specialisation = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == idSpecialisation);

                if (specialisation == null)
                {
                    return NotFound($"Specialisation with ID {idSpecialisation} not found.");
                }

                var dto = new GetSpecialisationDto()
                {
                    IdSpecialization = specialisation.IdSpecialization,
                    Name = specialisation.Name
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetSpecialisations()
        {
            try
            {
                var specialisations = _context.Specializations.ToList();

                var specs = new List<GetSpecialisationDto>();

                foreach (var s in specialisations)
                {
                    var dto = new GetSpecialisationDto()
                    {
                        IdSpecialization = s.IdSpecialization,
                        Name = s.Name
                    };

                    specs.Add(dto);
                }

                return Ok(specs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CreateSpecialisation(CreateSpecDto dto)
        {
            try
            {
                var specialisation = new Specialization()
                {
                    Name = dto.Name
                };

                _context.Specializations.Add(specialisation);
                _context.SaveChanges();

                return Ok(specialisation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[action]/{idSpecialisation}")]
        public ActionResult UpdateSpecialisation(int idSpecialisation, UpdateSpecDto dto)
        {
            try
            {

                var specialisation = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == idSpecialisation);

                if (specialisation == null)
                {
                    return NotFound($"Specialisation with ID {idSpecialisation} not found.");
                }

                specialisation.Name = string.IsNullOrWhiteSpace(dto.Name) ? specialisation.Name : dto.Name;

                _context.Specializations.Update(specialisation);
                _context.SaveChanges();

                return Ok(specialisation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("[action]/{idSpecialisation}")]
        public ActionResult DeleteSpecialisation(int idSpecialisation)
        {
            try
            {
                var specialisation = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == idSpecialisation);

                if (specialisation == null)
                {
                    return NotFound($"Specialisation with ID {idSpecialisation} not found.");
                }

                _context.Specializations.Remove(specialisation);
                _context.SaveChanges();

                return Ok($"Specialisation with ID {idSpecialisation} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
