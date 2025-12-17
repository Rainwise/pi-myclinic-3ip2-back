using myclinic_back.DTOs;

namespace myclinic_back.Interfaces
{
    public interface IPatientService : IReadService<GetPatientDto>, ICreateService<PatientDto>, IUpdateService<PatientDto>, IDeleteService
    { 
    }
}
