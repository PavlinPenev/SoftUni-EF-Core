using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MusicHub.Data.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public decimal Price => Songs.Sum(song => song.Price);

        public int? ProducerId { get; set; }

        #region Relations
        public virtual Producer Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        #endregion
    }
}
