using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Interfaces
{
    public interface ICreateService<T, TDto>
    {
        Task CreateObjectAsync(TDto dto);
        T CreateObject(TDto input);
    }
}
