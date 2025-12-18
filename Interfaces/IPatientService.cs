using myclinic_back.DTOs;
using myclinic_back.Models;

namespace myclinic_back.Interfaces
{
    public interface IPatientService : IReadService<Patient, GetPatientDto>, ICreateService<Patient, PatientDto>, IUpdateService<Patient, PatientDto>, IDeleteService
    { 
    }
}
