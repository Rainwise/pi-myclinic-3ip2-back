using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myclinic_back.DTOs;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Services;
using System.Numerics;

namespace myclinic_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPatientService _patientService;

        public PatientController(IConfiguration configuration, IPatientService patientService)
        {
            _configuration = configuration;
            _patientService = patientService;
        }

        [HttpGet("{idPatient}")]
        public async Task<ActionResult> GetPatientById(int idPatient)
        {
            try
            {
                var patient = await _patientService.GetByIdAsync(idPatient);

                return patient is null ? NotFound() : Ok(patient);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPatients()
        {
            try
            {
                var patients = await _patientService.GetAllAsync();

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient(PatientDto dto)
        {
            try
            {
                await _patientService.CreateObjectAsync(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{idPatient}")]
        public async Task<ActionResult> UpdatePatient(int idPatient, PatientDto dto)
        {
            try
            {
                await _patientService.UpdateObjectAsync(idPatient, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{idPatient}")]
        public async Task<ActionResult> DeletePatient(int idPatient)
        {
            try
            {
                await _patientService.DeleteObjectAsync(idPatient);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
