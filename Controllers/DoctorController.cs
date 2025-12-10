using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myclinic_back.DTOs;
using myclinic_back.Services;

namespace myclinic_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // All endpoints require authentication
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);

        if (doctor == null)
            return NotFound(new { message = $"Doctor with ID {id} not found" });

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto createDoctorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var doctor = await _doctorService.CreateAsync(createDoctorDto);
            return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto updateDoctorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var doctor = await _doctorService.UpdateAsync(id, updateDoctorDto);

            if (doctor == null)
                return NotFound(new { message = $"Doctor with ID {id} not found" });

            return Ok(doctor);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _doctorService.DeleteAsync(id);

        if (!result)
            return NotFound(new { message = $"Doctor with ID {id} not found" });

        return NoContent();
    }
}