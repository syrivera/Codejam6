namespace Api.Models;

public class Meal
{
    public int meal_id { get; set; }
    public required string name { get; set; }
    public int calories { get; set; }
    public int carbs { get; set; }
    public int fat {  get; set; }
    public int protein { get; set; }
    public DateTime date { get; set; }
}