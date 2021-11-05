using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaseController.Controllers
{
    [ApiController]
    [Route("api/weather-forecast")]
    public class WeatherForecastController : BaseController<WeatherForecast>
    {
        public WeatherForecastController(BaseControllerDBContext context) : base(context)
        {
        
        }

        [HttpGet]
        public override async Task<IEnumerable<WeatherForecast>> GetAllAsync(int count = -1, int skip = -1, string searchTerm = null, string orderBy = null)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                return await _dbSet.Where(d => d.Summary.Contains(searchTerm)).ToListAsync();
            }

            if (!String.IsNullOrEmpty(orderBy))
            {
                return await _dbSet.AsQueryable().OrderBy(orderBy).ToListAsync();
            }

            return await base.GetAllAsync(count, skip, searchTerm);
        }
    }
}
