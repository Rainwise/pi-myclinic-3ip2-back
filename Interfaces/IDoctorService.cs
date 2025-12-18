using myclinic_back.DTOs;
using myclinic_back.Models;

namespace myclinic_back.Interfaces
{
    public interface IDoctorService : IReadService<Doctor, GetDoctorDto>, ICreateService<Doctor, DoctorDto>, IUpdateService<Doctor, DoctorDto>, IDeleteService
    {
    }
}
