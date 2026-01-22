namespace Api.Models;

public class UserProgress
{
    [Key]
    [Column("progress_id")]
    public int progress_id { get; set; }

    public double current_weight { get; set; }
    public double target_weight { get; set; }
    public int consumed_calories { get; set; }
    public int target_cals { get; set; }
}