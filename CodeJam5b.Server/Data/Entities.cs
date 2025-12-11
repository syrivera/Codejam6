namespace CodeJam5b.Server.Data
{
    public class MealEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Calories { get; set; }
        public int Carbs { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public DateOnly Date { get; set; }
    }

    public class ProgressEntity
    {
        public int Id { get; set; }
        public int CurrentWeight { get; set; }
        public int TargetWeight { get; set; }
        public int TargetCals { get; set; }
        public int TargetCarbs { get; set; }
        public int TargetFat { get; set; }
        public int TargetProtein { get; set; }
    }
}
