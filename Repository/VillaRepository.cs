﻿using MagicVillaAPI.Datos;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;

namespace MagicVillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public VillaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Villa> Update(Villa entidad)
        {
           entidad.FechaActualizacion=DateTime.Now;
            _dbContext.Villas.Update(entidad);
            await RecordVilla();
            return entidad; 
        }
        public async Task RecordVilla()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
