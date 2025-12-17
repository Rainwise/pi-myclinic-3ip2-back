using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Interfaces
{
    public interface IUpdateService<TDto>
    {
        Task UpdateObjectAsync(int id, TDto dto);
    }
}
