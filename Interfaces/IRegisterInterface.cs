using myclinic_back.DTOs;

namespace myclinic_back.Interfaces
{
    public interface IRegisterInterface<T, TDto>
    {
        void RegisterUser(TDto dto);
        T Register(TDto dto);
    }
}
