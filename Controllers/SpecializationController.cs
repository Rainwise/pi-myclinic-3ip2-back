using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myclinic_back.Dtos;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Services;
using System.Threading.Tasks;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecializationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISpecializationService _specService;

        public SpecializationController(IConfiguration configuration, ISpecializationService specializationService)
        {
            _configuration = configuration;
            _specService = specializationService;
        }

        [HttpGet("{idSpecialisation}")]
        public async Task<ActionResult> GetSpecialisationById(int idSpecialisation)
        {
            try
            {
                var specialisation = await _specService.GetByIdAsync(idSpecialisation);
                
                return specialisation is null ? NotFound() : Ok(specialisation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetSpecialisations()
        {
            try
            {
                var specialisations = await _specService.GetAllAsync();

                return Ok(specialisations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateSpecialisation(SpecDto dto)
        {
            try
            {
                
                await _specService.CreateObjectAsync(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{idSpecialisation}")]
        public async Task<ActionResult> UpdateSpecialisation(int idSpecialisation, SpecDto dto)
        {
            try
            {
                await _specService.UpdateObjectAsync(idSpecialisation, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("{idSpecialisation}")]
        public async Task<ActionResult> DeleteSpecialisation(int idSpecialisation)
        {
            try
            {
                await _specService.DeleteObjectAsync(idSpecialisation);  

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
