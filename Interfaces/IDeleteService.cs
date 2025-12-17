using Microsoft.AspNetCore.Mvc;

namespace myclinic_back.Interfaces
{
    public interface IDeleteService
    {
        Task DeleteObjectAsync(int id);
    }
}
