using CDI_Tool.Data;
using CDI_Tool.Model;
using Microsoft.EntityFrameworkCore;

namespace CDI_Tool.Repository
{
      public class GenericRepository<T>(CDIDBContext _context) : IRepository<T> where T : class, IEntity
    {
        public async Task<T> Add(T entity)
        {
            
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Delete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                return null;
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>?> GetAll()
        {
            return await _context.Set<T>().ToListAsync();

        }
        public async Task<IEnumerable<T>?> GetAllV2()
        {
            return await _context.Set<T>().ToListAsync();

        }

        public async Task<T?> GetById(int id)
        {
            T? entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}