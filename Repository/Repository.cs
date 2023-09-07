using MagicVillaAPI.Datos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDbContext dbContext)
        {
           _dbContext= dbContext;
            this.DbSet=_dbContext.Set<T>();
        }
        public async Task Create(T entidad)
        {
            await DbSet.AddAsync(entidad);
            await Record();
        }

        public async Task Delete(T entidad)
        {
             DbSet.Remove(entidad);
            await Record();
        }

        public  async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = DbSet;
            if(!tracked)
            {
                query = query.AsNoTracking();
            }
            if(filter != null)
            {
                query= query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
           IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Record()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
