using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell()
        {
            Guns = new HashSet<Gun>();
        }

        public int Id { get; set; }

        [Required]
        [Range(Constants.SHELL_WEIGHT_MIN_VALUE, Constants.SHELL_WEIGHT_MAX_VALUE)]
        public double ShellWeight { get; set; }

        [Required]
        [MinLength(Constants.CALIBER_MIN_LENGTH)]
        [MaxLength(Constants.CALIBER_MAX_LENGTH)]
        public string Caliber { get; set; }

        #region Navigation Properties
        public ICollection<Gun> Guns { get; set; }
        #endregion
    }
}
