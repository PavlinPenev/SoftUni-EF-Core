using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public int PlayerId { get; set; }

        [Required]
        public int ScoredGoals { get; set; }

        [Required]
        public int Assists { get; set; }

        [Required]
        public int MinutesPlayed { get; set; }

        #region Relations
        public virtual Player Player { get; set; }

        public virtual Game Game { get; set; }
        #endregion
    }
}
