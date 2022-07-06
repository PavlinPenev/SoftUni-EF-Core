using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int SquadNumber { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public bool IsInjured { get; set; }

        #region Relations
        public virtual Team Team { get; set; }

        public virtual Position Position { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }
        #endregion
    }
}
