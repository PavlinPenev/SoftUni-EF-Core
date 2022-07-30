namespace Artillery.Data.Models
{
    public class CountryGun
    {
        public int CountryId { get; set; }

        public int GunId { get; set; }

        #region Navigation Properties
        public Gun Gun { get; set; }

        public Country Country { get; set; }
        #endregion
    }
}
