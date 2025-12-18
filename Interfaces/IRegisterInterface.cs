using myclinic_back.DTOs;

namespace myclinic_back.Interfaces
{
    public interface IRegisterInterface
    {
        void RegisterUser(RegisterDto dto);
    }
}
