using myclinic_back.DTOs;
using myclinic_back.Models;

namespace myclinic_back.Interfaces
{
    public interface IAdminService : ILoginInterface<Admin, LoginDto, LoginResponseDto>, IRegisterInterface<Admin, RegisterDto>
    {
    }
}
