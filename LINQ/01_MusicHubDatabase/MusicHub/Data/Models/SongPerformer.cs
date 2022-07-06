using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        public int SongId { get; set; }

        public int PerformerId { get; set; }

        #region Relations
        [Required]
        public virtual Song Song { get; set; }

        [Required]
        public virtual Performer Performer { get; set; }
        #endregion
    }
}
