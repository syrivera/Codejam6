using CodeJam5b.Server.Mapping;
using CodeJam5b.Server.Models;
using CodeJam5b.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeJam5b.Server.Controllers
{
    [ApiController]
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly AppDb _db;
        public ProgressController(AppDb db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<ProgressData>> Get()
        {
            var entity = await _db.Progress.FindAsync(1) ?? new Data.ProgressEntity { Id = 1 };
            return Map.ToDto(entity);
        }

        [HttpPut]
        public async Task<ActionResult<ProgressData>> Update(ProgressData body)
        {
            var entity = await _db.Progress.FindAsync(1);
            if (entity is null)
            {
                entity = new Data.ProgressEntity { Id = 1 };
                _db.Progress.Add(entity);
            }
            Map.Apply(entity, body);
            await _db.SaveChangesAsync();
            return Map.ToDto(entity);
        }
    }
}
