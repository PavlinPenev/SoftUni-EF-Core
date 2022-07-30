using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(Constants.COUNTRY_NAME_MIN_LENGTH)]
        [MaxLength(Constants.COUNTRY_NAME_MAX_LENGTH)]
        public string CountryName { get; set; }

        [Required]
        [Range(Constants.ARMY_SIZE_MIN_COUNT, Constants.ARMY_SIZE_MAX_COUNT)]
        public int ArmySize { get; set; }

        #region Navigation Properties
        public ICollection<CountryGun> CountriesGuns { get; set; }
        #endregion
    }
}
