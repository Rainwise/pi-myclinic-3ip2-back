using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Interfaces
{
    public interface ICreateService<TDto>
    {
        Task CreateObjectAsync(TDto dto);
    }
}
