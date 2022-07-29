using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Models
{
    public class Author
    {
        public Author()
        {
            AuthorsBooks = new List<AuthorBook>();
        }

        public int Id { get; set; }

        [MinLength(Constants.MIN_LENGTH_NAME)]
        [MaxLength(Constants.MAX_LENGTH_NAME)]
        [Required]
        public string FirstName { get; set; }

        [MinLength(Constants.MIN_LENGTH_NAME)]
        [MaxLength(Constants.MAX_LENGTH_NAME)]
        [Required]
        public string LastName { get; set;}

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Constants.PHONE_PATTERN)]
        public string Phone { get; set; }

        #region Navigation Properties
        public ICollection<AuthorBook> AuthorsBooks { get; set; }
        #endregion
    }
}
