using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }

        [Required]
        public int TheatreId { get; set; }

        #region Navigation Properties
        public Play Play { get; set; }

        public Theatre Theatre { get; set; }
        #endregion
    }
}
