using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Writer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public string Pseudonym { get; set; }

        #region Relations
        public virtual ICollection<Song> Songs { get; set; }
        #endregion
    }
}
