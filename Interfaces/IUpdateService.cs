using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Interfaces
{
    public interface IUpdateService<T, TDto>
    {
        Task UpdateObjectAsync(int id, TDto dto);

        T UpdateObject(T obj, TDto dto);
    }
}
