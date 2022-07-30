using Artillery.Data.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        public int Id { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [Range(Constants.GUN_MIN_WEIGHT, Constants.GUN_MAX_WEIGHT)]
        public int GunWeight { get; set;}

        [Required]
        [Range(Constants.BARREL_MIN_LENGTH, Constants.BARREL_MAX_LENGTH)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        [Range(Constants.RANGE_MIN_VALUE, Constants.RANGE_MAX_VALUE)]
        public int Range { get; set; }

        [Required]
        public GunType GunType { get; set; }

        [Required]
        public int ShellId { get; set; }

        #region Navigation Properties
        public Manufacturer Manufacturer { get; set; }

        public Shell Shell { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }
        #endregion
    }
}
