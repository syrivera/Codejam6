using Microsoft.AspNetCore.Mvc;
using CodeJam5b.Server.Models;
using CodeJam5b.Server.Mapping;
using CodeJam5b.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeJam5b.Server.Controllers
{
    [ApiController]
    [Route("api/meals")]
    public class CalorieCounterController : ControllerBase
    {
        private readonly AppDb _db;
        public CalorieCounterController(AppDb db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetAll([FromQuery] string? date)
        {
            IQueryable<Data.MealEntity> q = _db.Meals;
            if (!string.IsNullOrWhiteSpace(date))
            {
                var d = DateOnly.Parse(date);
                q = q.Where(m => m.Date == d);
            }
            var list = await q.OrderByDescending(m => m.Date).ToListAsync();
            return list.Select(Map.ToDto).ToList();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MealEntry>> GetById(int id)
        {
            var entity = await _db.Meals.FindAsync(id);
            return entity is null ? NotFound() : Map.ToDto(entity);
        }

        [HttpPost]
        public async Task<ActionResult<MealEntry>> Create(MealEntry body)
        {
            var entity = Map.ToEntity(body);
            _db.Meals.Add(entity);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MealEntry body)
        {
            var entity = await _db.Meals.FindAsync(id);
            if (entity is null) return NotFound();
            var updated = Map.ToEntity(body);
            entity.Name = updated.Name;
            entity.Calories = updated.Calories;
            entity.Carbs = updated.Carbs;
            entity.Fat = updated.Fat;
            entity.Protein = updated.Protein;
            entity.Date = updated.Date;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Meals.FindAsync(id);
            if (entity is null) return NotFound();
            _db.Meals.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<ActionResult<object>> Summary([FromQuery] string date)
        {
            var d = DateOnly.Parse(date);
            var items = await _db.Meals.Where(m => m.Date == d).ToListAsync();
            var totals = new { totalCalories = items.Sum(e => e.Calories), totalCarbs = items.Sum(e => e.Carbs), totalFat = items.Sum(e => e.Fat), totalProtein = items.Sum(e => e.Protein) };
            var progress = await _db.Progress.FindAsync(1);
            var targetCals = progress?.TargetCals ?? 0;
            return new { date, totals.totalCalories, totals.totalCarbs, totals.totalFat, totals.totalProtein, targetCals, remaining = targetCals > 0 ? targetCals - totals.totalCalories : 0 };
        }
    }
}
