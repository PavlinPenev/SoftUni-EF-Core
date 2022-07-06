using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        [StringLength(3)]
        public string Initials { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        public int PrimaryKitColorId { get; set; }

        [Required]
        public int SecondaryKitColorId { get; set; }

        [Required]
        public int TownId { get; set; }

        #region Relations
        public Color PrimaryKitColor { get; set; }

        public Color SecondaryKitColor { get; set; }

        public Town Town { get; set; }

        public ICollection<Game> HomeGames { get; set; }
        
        public ICollection<Game> AwayGames { get; set; }

        public ICollection<Player> Players { get; set; }
        #endregion
    }
}
