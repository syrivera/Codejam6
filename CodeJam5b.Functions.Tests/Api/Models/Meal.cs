using System;

namespace Api.Models;

// Minimal model used by function/unit tests.
public class Meal
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Calories { get; set; }
    public DateTime Date { get; set; }
}
