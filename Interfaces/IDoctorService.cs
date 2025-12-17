using myclinic_back.DTOs;

namespace myclinic_back.Interfaces
{
    public interface IDoctorService : IReadService<GetDoctorDto>, ICreateService<DoctorDto>, IUpdateService<DoctorDto>, IDeleteService
    {
    }
}
