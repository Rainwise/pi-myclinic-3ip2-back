using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Interfaces
{
    public interface IReadService<TDto>
    {
        Task<TDto> GetByIdAsync(int id);
        Task<List<TDto>> GetAllAsync();
    }
}
