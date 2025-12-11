using CodeJam5b.Server.Data;
using CodeJam5b.Server.Models;

namespace CodeJam5b.Server.Mapping
{
    public static class Map
    {
        public static MealEntry ToDto(MealEntity e) => new MealEntry
        {
            Name = e.Name,
            Calories = e.Calories,
            Carbs = e.Carbs,
            Fat = e.Fat,
            Protein = e.Protein,
            Date = e.Date.ToString("yyyy-MM-dd")
        };

        public static MealEntity ToEntity(MealEntry d) => new MealEntity
        {
            Name = d.Name,
            Calories = d.Calories,
            Carbs = d.Carbs,
            Fat = d.Fat,
            Protein = d.Protein,
            Date = DateOnly.Parse(d.Date)
        };

        public static ProgressData ToDto(ProgressEntity e) => new ProgressData
        {
            CurrentWeight = e.CurrentWeight,
            TargetWeight = e.TargetWeight,
            TargetCals = e.TargetCals,
            TargetCarbs = e.TargetCarbs,
            TargetFat = e.TargetFat,
            TargetProtein = e.TargetProtein
        };

        public static void Apply(ProgressEntity e, ProgressData d)
        {
            e.CurrentWeight = d.CurrentWeight;
            e.TargetWeight = d.TargetWeight;
            e.TargetCals = d.TargetCals;
            e.TargetCarbs = d.TargetCarbs;
            e.TargetFat = d.TargetFat;
            e.TargetProtein = d.TargetProtein;
        }
    }
}
