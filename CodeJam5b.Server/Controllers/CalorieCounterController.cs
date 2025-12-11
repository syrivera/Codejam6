using Microsoft.AspNetCore.Mvc;
using CodeJam5b.Models;
using CodeJam5b.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeJam5b.Server.Controllers
{
    public class MealEntry
    {
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        
        [Range(0, 10000, ErrorMessage = "Calories must be between 0 and 10000")]
        public int Calories { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Carbs must be between 0 and 1000")]
        public int Carbs { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Fat must be between 0 and 1000")]
        public int Fat { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Protein must be between 0 and 1000")]
        public int Protein { get; set; }
        
        public string? DateTime { get; set; }
    }

    [ApiController]
    [Route("api/meals")]
    public class CalorieCounterController : ControllerBase
    {
        private readonly CalorieCounterContext _db;
        public CalorieCounterController(CalorieCounterContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealEntry>>> GetAll()
        {
            var meals = await _db.Meals.ToListAsync();
            return meals.Select(m => new MealEntry
            {
                Id = m.MealId,
                Name = m.MealName,
                Calories = m.Calories,
                Carbs = m.Carbs,
                Fat = m.Fat,
                Protein = m.Protein,
                DateTime = null
            }).ToList();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MealEntry>>> SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await GetAll();
            }

            var meals = await _db.Meals
                .Where(m => m.MealName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
                
            return meals.Select(m => new MealEntry
            {
                Id = m.MealId,
                Name = m.MealName,
                Calories = m.Calories,
                Carbs = m.Carbs,
                Fat = m.Fat,
                Protein = m.Protein,
                DateTime = null
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealEntry>> GetById(string id)
        {
            var meal = await _db.Meals.FindAsync(id);
            if (meal is null) return NotFound();
            
            return new MealEntry
            {
                Id = meal.MealId,
                Name = meal.MealName,
                Calories = meal.Calories,
                Carbs = meal.Carbs,
                Fat = meal.Fat,
                Protein = meal.Protein,
                DateTime = null
            };
        }

        [HttpPost]
        public async Task<ActionResult<MealEntry>> Create(MealEntry body)
        {
            var meal = new Meal
            {
                MealId = Guid.NewGuid().ToString(),
                MealName = body.Name,
                Calories = body.Calories,
                Carbs = body.Carbs,
                Fat = body.Fat,
                Protein = body.Protein
            };
            
            _db.Meals.Add(meal);
            await _db.SaveChangesAsync();
            
            body.Id = meal.MealId;
            return CreatedAtAction(nameof(GetById), new { id = meal.MealId }, body);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, MealEntry body)
        {
            var meal = await _db.Meals.FindAsync(id);
            if (meal is null) return NotFound();
            
            meal.MealName = body.Name;
            meal.Calories = body.Calories;
            meal.Carbs = body.Carbs;
            meal.Fat = body.Fat;
            meal.Protein = body.Protein;
            
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var meal = await _db.Meals.FindAsync(id);
            if (meal is null) return NotFound();
            
            _db.Meals.Remove(meal);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<ActionResult<object>> Summary()
        {
            var meals = await _db.Meals.ToListAsync();
            var progress = await _db.Progress.FirstOrDefaultAsync();
            
            var totalCalories = meals.Sum(m => m.Calories);
            var totalCarbs = meals.Sum(m => m.Carbs);
            var totalFat = meals.Sum(m => m.Fat);
            var totalProtein = meals.Sum(m => m.Protein);
            var targetCals = progress?.TargetCals ?? 0;
            
            return new
            {
                totalCalories,
                totalCarbs,
                totalFat,
                totalProtein,
                targetCals,
                remaining = targetCals > 0 ? targetCals - totalCalories : 0
            };
        }
    }
}
