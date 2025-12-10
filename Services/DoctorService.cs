using Microsoft.EntityFrameworkCore;
using myclinic_back.Data;
using myclinic_back.DTOs;
using myclinic_back.Models;

namespace myclinic_back.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(int id);
    Task<DoctorDto> CreateAsync(CreateDoctorDto createDoctorDto);
    Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto updateDoctorDto);
    Task<bool> DeleteAsync(int id);
}

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _context;

    public DoctorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        var doctors = await _context.Doctors
            .OrderBy(d => d.LastName)
            .ThenBy(d => d.FirstName)
            .ToListAsync();

        return doctors.Select(MapToDto);
    }

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        return doctor == null ? null : MapToDto(doctor);
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto createDoctorDto)
    {
        var doctor = new Doctor
        {
            FirstName = createDoctorDto.FirstName,
            LastName = createDoctorDto.LastName,
            Email = createDoctorDto.Email,
            PhoneNumber = createDoctorDto.PhoneNumber,
            Specialization = createDoctorDto.Specialization,
            LicenseNumber = createDoctorDto.LicenseNumber,
            IsActive = true,
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        return MapToDto(doctor);
    }

    public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto updateDoctorDto)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
            return null;

        // Update only provided fields
        if (updateDoctorDto.FirstName != null)
            doctor.FirstName = updateDoctorDto.FirstName;

        if (updateDoctorDto.LastName != null)
            doctor.LastName = updateDoctorDto.LastName;

        if (updateDoctorDto.Email != null)
            doctor.Email = updateDoctorDto.Email;

        if (updateDoctorDto.PhoneNumber != null)
            doctor.PhoneNumber = updateDoctorDto.PhoneNumber;

        if (updateDoctorDto.Specialization != null)
            doctor.Specialization = updateDoctorDto.Specialization;

        if (updateDoctorDto.LicenseNumber != null)
            doctor.LicenseNumber = updateDoctorDto.LicenseNumber;

        if (updateDoctorDto.IsActive.HasValue)
            doctor.IsActive = updateDoctorDto.IsActive.Value;

        await _context.SaveChangesAsync();

        return MapToDto(doctor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
            return false;

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();

        return true;
    }

    private static DoctorDto MapToDto(Doctor doctor)
    {
        return new DoctorDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Email = doctor.Email,
            PhoneNumber = doctor.PhoneNumber,
            Specialization = doctor.Specialization,
            LicenseNumber = doctor.LicenseNumber,
            IsActive = doctor.IsActive,
        };
    }
}