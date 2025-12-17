using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Services;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace myclinic_back.Controllers
{
    [Route("api/Doctors")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDoctorService _doctorService;

        public DoctorController(IConfiguration configuration, IDoctorService doctor)
        {
            _configuration = configuration;
            _doctorService = doctor;
        }

        [HttpGet("{idDoctor}")]
        public async Task<ActionResult> GetDoctorById(int idDoctor)
        {
            try
            {
                var doctor = await _doctorService.GetByIdAsync(idDoctor);

                return doctor is null ? NotFound() : Ok(doctor);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetDoctors()
        {
            try
            {
                var doctors = await _doctorService.GetAllAsync();

                return Ok(doctors);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateDoctor(DoctorDto dto)
        {
            try
            {
                await _doctorService.CreateObjectAsync(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{idDoctor}")]
        public async Task<ActionResult> UpdateDoctor(int idDoctor, DoctorDto dto)
        {
            try
            {
                await _doctorService.UpdateObjectAsync(idDoctor, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{idDoctor}")]
        public async Task<ActionResult> DeleteDoctor(int idDoctor)
        {
            try
            {
                await _doctorService.DeleteObjectAsync(idDoctor);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
