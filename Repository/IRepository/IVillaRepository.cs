using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository
{
    public interface IVillaRepository:IRepository<Villa>
    {
        Task<Villa> Update(Villa entidad);
    }
}
