using myclinic_back.DTOs;

namespace myclinic_back.Interfaces
{
    public interface ILoginInterface
    {
        Task<LoginResponseDto> LoginUserAsync(LoginDto dto);
    }
}
