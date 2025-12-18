using myclinic_back.Dtos;
using myclinic_back.Models;

namespace myclinic_back.Interfaces
{
    public interface ISpecializationService : IReadService<Specialization, GetSpecialisationDto>, ICreateService<Specialization, SpecDto>, IUpdateService<Specialization, SpecDto>, IDeleteService
    {
    }
}
