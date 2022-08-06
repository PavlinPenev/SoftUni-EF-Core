using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        [Required]
        public int TeamId { get; set; }

        [Required]
        public int FootballerId { get; set; }

        #region Nav props
        public Team Team { get; set; }

        public Footballer Footballer { get; set; }
        #endregion
    }
}
