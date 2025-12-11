using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeJam5b.Models
{
    [Table("progress")]
    public class UserProgress
    {
        [Key]
        [Column("progress_id")]
        public required string ProgressId { get; set; }

        [Column("current_weight")]
        public int CurrentWeight { get; set; }

        [Column("target_weight")]
        public int TargetWeight { get; set; }

        [Column("target_cals")]
        public int TargetCals { get; set; }

        [Column("target_carbs")]
        public int TargetCarbs { get; set; }

        [Column("target_fat")]
        public int TargetFat { get; set; }

        [Column("target_protein")]
        public int TargetProtein { get; set; }

        [Column("consumed_calories")]
        public int ConsumedCalories { get; set; }

        [Column("consumed_carbs")]
        public int ConsumedCarbs { get; set; }

        [Column("consumed_fat")]
        public int ConsumedFat { get; set; }

        [Column("consumed_protein")]
        public int ConsumedProtein { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}
