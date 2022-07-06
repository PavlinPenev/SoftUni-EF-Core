using P01_StudentSystem.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public ResourceTypeEnum ResourceType { get; set; }

        [Required]
        public int CourseId { get; set; }

        #region Relations
        public Course Course { get; set; }
        #endregion
    }
}
