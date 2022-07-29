using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType(Constants.BOOK)]
    public class ImportBookDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(Constants.MIN_LENGTH_NAME)]
        [MaxLength(Constants.MAX_LENGTH_NAME)]
        public string Name { get; set; }

        [XmlElement(nameof(Genre))]
        [Required]
        [Range(Constants.MIN_GENRE_VALUE, Constants.MAX_GENRE_VALUE)]
        public int Genre { get; set; }

        [XmlElement(nameof(Price))]
        [Range((double)Constants.PRICE_MIN_VALUE, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [XmlElement(nameof(Pages))]
        [Range(Constants.PAGE_MIN_COUNT, Constants.PAGE_MAX_COUNT)]
        public int Pages { get; set; }

        [XmlElement(nameof(PublishedOn))]
        [Required]
        public string PublishedOn { get; set; }
    }
}
