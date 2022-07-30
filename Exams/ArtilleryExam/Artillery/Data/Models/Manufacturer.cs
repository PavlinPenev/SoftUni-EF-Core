using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MANUFACTURER_NAME_MIN_LENGTH)]
        [MaxLength(Constants.MANUFACTURER_NAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }

        [Required]
        [MinLength(Constants.FOUNDED_MIN_LENGTH)]
        [MaxLength(Constants.FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }

        #region Navigation Properties
        public ICollection<Gun> Guns { get; set; }
        #endregion
    }
}
