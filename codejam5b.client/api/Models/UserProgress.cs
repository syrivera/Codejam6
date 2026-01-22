using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

public class UserProgress
{
    [Key]
    [Column("progress_id")]
    public int progress_id { get; set; }

    public int current_weight { get; set; }
    public int target_weight { get; set; }
    public int target_cals { get; set; }
    public int target_carbs { get; set; }
    public int target_fat { get; set; }
    public int target_protein { get; set; }

    public int consumed_cals { get; set; }
    public int consumed_carbs { get; set; }
    public int consumed_fat { get; set; }
    public int consumed_protein { get; set; }
}