using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaseController.Controllers
{
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity : BaseModel
    {
        protected readonly BaseControllerDBContext _context;
        protected DbSet<TEntity> _dbSet { get; set; }
        public BaseController(BaseControllerDBContext context) 
        { 
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        [HttpGet]
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int count = -1, int skip = -1, string searchTerm = null, string sortBy = null)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                throw new Exception("Cannot search in base controller.");
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                throw new Exception("Cannot sort in base controller.");
            }

            if (count != -1 && skip != -1)
            {
                return await _dbSet.Skip(skip).Take(count).ToListAsync();
            }

            return await _dbSet.ToListAsync();
        }

        [HttpGet("{id}")]
        public virtual async Task<TEntity> GetAsync(Guid id)
        {           
            return await _dbSet.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {           
            var entity = await _dbSet.Where(e => e.Id == id).FirstOrDefaultAsync();
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {           
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        [HttpPut]
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {           
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
