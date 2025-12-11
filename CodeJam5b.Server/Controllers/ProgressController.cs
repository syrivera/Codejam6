using Microsoft.AspNetCore.Mvc;
using CodeJam5b.Models;
using CodeJam5b.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeJam5b.Server.Controllers
{
    public class ProgressData
    {
        public string? Id { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Current weight must be between 0 and 1000")]
        public int CurrentWeight { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Target weight must be between 0 and 1000")]
        public int TargetWeight { get; set; }
        
        [Range(0, 10000, ErrorMessage = "Target calories must be between 0 and 10000")]
        public int TargetDailyCalories { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Target carbs must be between 0 and 1000")]
        public int TargetDailyCarbs { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Target fat must be between 0 and 1000")]
        public int TargetDailyFat { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Target protein must be between 0 and 1000")]
        public int TargetDailyProtein { get; set; }

        // Current day consumed values
        public int ConsumedCalories { get; set; }
        public int ConsumedCarbs { get; set; }
        public int ConsumedFat { get; set; }
        public int ConsumedProtein { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class AddMealNutrientsRequest
    {
        public int Calories { get; set; }
        public int Carbs { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
    }

    [ApiController]
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly CalorieCounterContext _db;
        public ProgressController(CalorieCounterContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<ProgressData>> Get()
        {
            var progress = await _db.Progress.FirstOrDefaultAsync();
            if (progress is null) return NotFound();
            
            // Reset consumed values if it's a new day
            var today = DateTime.UtcNow.Date;
            if (progress.LastUpdated.Date != today)
            {
                progress.ConsumedCalories = 0;
                progress.ConsumedCarbs = 0;
                progress.ConsumedFat = 0;
                progress.ConsumedProtein = 0;
                progress.LastUpdated = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
            
            return new ProgressData
            {
                Id = progress.ProgressId,
                CurrentWeight = progress.CurrentWeight,
                TargetWeight = progress.TargetWeight,
                TargetDailyCalories = progress.TargetCals,
                TargetDailyCarbs = progress.TargetCarbs,
                TargetDailyFat = progress.TargetFat,
                TargetDailyProtein = progress.TargetProtein,
                ConsumedCalories = progress.ConsumedCalories,
                ConsumedCarbs = progress.ConsumedCarbs,
                ConsumedFat = progress.ConsumedFat,
                ConsumedProtein = progress.ConsumedProtein,
                LastUpdated = progress.LastUpdated
            };
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProgressData body)
        {
            var progress = await _db.Progress.FirstOrDefaultAsync();
            if (progress is null)
            {
                // Create new progress record
                progress = new UserProgress
                {
                    ProgressId = Guid.NewGuid().ToString(),
                    CurrentWeight = body.CurrentWeight,
                    TargetWeight = body.TargetWeight,
                    TargetCals = body.TargetDailyCalories,
                    TargetCarbs = body.TargetDailyCarbs,
                    TargetFat = body.TargetDailyFat,
                    TargetProtein = body.TargetDailyProtein
                };
                _db.Progress.Add(progress);
            }
            else
            {
                progress.CurrentWeight = body.CurrentWeight;
                progress.TargetWeight = body.TargetWeight;
                progress.TargetCals = body.TargetDailyCalories;
                progress.TargetCarbs = body.TargetDailyCarbs;
                progress.TargetFat = body.TargetDailyFat;
                progress.TargetProtein = body.TargetDailyProtein;
            }
            
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("add-meal")]
        public async Task<ActionResult<ProgressData>> AddMealNutrients(AddMealNutrientsRequest body)
        {
            var progress = await _db.Progress.FirstOrDefaultAsync();
            if (progress is null) return NotFound("Progress record not found");

            // Reset consumed values if it's a new day
            var today = DateTime.UtcNow.Date;
            if (progress.LastUpdated.Date != today)
            {
                progress.ConsumedCalories = 0;
                progress.ConsumedCarbs = 0;
                progress.ConsumedFat = 0;
                progress.ConsumedProtein = 0;
            }

            // Add the meal nutrients to consumed totals
            progress.ConsumedCalories += body.Calories;
            progress.ConsumedCarbs += body.Carbs;
            progress.ConsumedFat += body.Fat;
            progress.ConsumedProtein += body.Protein;
            progress.LastUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return new ProgressData
            {
                Id = progress.ProgressId,
                CurrentWeight = progress.CurrentWeight,
                TargetWeight = progress.TargetWeight,
                TargetDailyCalories = progress.TargetCals,
                TargetDailyCarbs = progress.TargetCarbs,
                TargetDailyFat = progress.TargetFat,
                TargetDailyProtein = progress.TargetProtein,
                ConsumedCalories = progress.ConsumedCalories,
                ConsumedCarbs = progress.ConsumedCarbs,
                ConsumedFat = progress.ConsumedFat,
                ConsumedProtein = progress.ConsumedProtein,
                LastUpdated = progress.LastUpdated
            };
        }
    }
}
