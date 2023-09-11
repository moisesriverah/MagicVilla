using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository
{
    public interface INumberVillaRepository:IRepository<NumberVilla>
    {
        Task<NumberVilla> Update(NumberVilla entidad);
        Task RecordVilla();
    }
}
