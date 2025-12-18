using myclinic_back.Dtos;

namespace myclinic_back.Interfaces
{
    public interface ISpecializationService : IReadService<GetSpecialisationDto>, ICreateService<SpecDto>, IUpdateService<SpecDto>, IDeleteService
    {
    }
}
