using myclinic_back.DTOs;

namespace myclinic_back.Interfaces
{
    public interface ILoginInterface<T, TDto, TResponse>
    {
        Task<TResponse> LoginUserAsync(TDto dto);

        TResponse Login(T obj, TDto dto);

    }
}
