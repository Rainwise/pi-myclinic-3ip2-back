using Microsoft.EntityFrameworkCore;
using myclinic_back.Dtos;
using myclinic_back.Interfaces;
using myclinic_back.Models;

namespace myclinic_back.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly PiProjectContext _context;

        public SpecializationService(PiProjectContext context)
        {
            _context = context;
        }

        public async Task<GetSpecialisationDto> GetByIdAsync(int id)
        {
            var specialisation = await _context.Specializations
                .FirstOrDefaultAsync(s => s.IdSpecialization == id);

            var dto = GetObject(specialisation);

            return dto;
        }

        public async Task<List<GetSpecialisationDto>> GetAllAsync()
        {
            var specialisations = _context.Specializations.ToList();

            var specs = new List<GetSpecialisationDto>();

            foreach (var s in specialisations)
            {
                var dto = GetObject(s);

                specs.Add(dto);
            }

            return specs;
        }

        public async Task CreateObjectAsync(SpecDto dto)
        {
           
            var specialization = CreateObject(dto);
            _context.Specializations.Add(specialization);
            _context.SaveChanges();
        }

        public async Task UpdateObjectAsync(int id, SpecDto dto)
        {
            var specialisation = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == id);
            _context.Specializations.Update(UpdateObject(specialisation, dto));
            _context.SaveChanges();
        }

        public async Task DeleteObjectAsync(int id)
        {
            var specialisation = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == id);

            var doctors = _context.Doctors.Where(d => d.SpecializationId == specialisation.IdSpecialization).ToList();

            foreach (var doctor in doctors)
            {
                _context.Remove(doctor);
            }

            _context.SaveChanges();

            _context.Specializations.Remove(specialisation);
            _context.SaveChanges();
        }

        public GetSpecialisationDto GetObject(Specialization specialisation)
        {
            var dto = new GetSpecialisationDto()
            {
                IdSpecialization = specialisation.IdSpecialization,
                Name = specialisation.Name
            };

            return dto;
        }

        public Specialization CreateObject(SpecDto dto)
        {
            var specialisation = new Specialization()
            {
                Name = dto.Name
            };

            return specialisation;
        }

        public Specialization UpdateObject(Specialization specialisation, SpecDto dto)
        {
            specialisation.Name = string.IsNullOrWhiteSpace(dto.Name) ? specialisation.Name : dto.Name;

            return specialisation;
        }
    }
}
