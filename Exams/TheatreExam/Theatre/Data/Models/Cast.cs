using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Cast
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        [RegularExpression(@"\+44-[0-9]{2}-[0-9]{3}-[0-9]{4}")]
        public string PhoneNumber { get; set; }

        [Required]
        public int PlayId { get; set; }

        #region Navigation Properties
        public Play Play { get; set; }
        #endregion
    }
}
