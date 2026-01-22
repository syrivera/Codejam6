namespace Api.Models;

// Minimal model used by function/unit tests.
public class UserProgress
{
    public int Id { get; set; }
    public double CurrentWeight { get; set; }
    public double GoalWeight { get; set; }
    public int CurrentCalories { get; set; }
    public int GoalCalories { get; set; }
}
