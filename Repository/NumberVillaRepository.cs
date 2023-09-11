using MagicVillaAPI.Datos;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;

namespace MagicVillaAPI.Repository
{
    public class NumberVillaRepository : Repository<NumberVilla>, INumberVillaRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public NumberVillaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NumberVilla> Update(NumberVilla entidad)
        {
           entidad.FechaActualizacion=DateTime.Now;
            _dbContext.NumeroVillas.Update(entidad);
            await RecordVilla();
            return entidad; 
        }
        public async Task RecordVilla()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
