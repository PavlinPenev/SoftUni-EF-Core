using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Data.Models
{
    public class Book
    {
        public Book()
        {
            AuthorsBooks = new List<AuthorBook>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MIN_LENGTH_NAME)]
        [MaxLength(Constants.MAX_LENGTH_NAME)]
        public string Name { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Range((double)Constants.PRICE_MIN_VALUE, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Range(Constants.PAGE_MIN_COUNT, Constants.PAGE_MAX_COUNT)]
        public int Pages { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        #region Navigation Properties
        public ICollection<AuthorBook> AuthorsBooks { get; set; }
        #endregion
    }
}
