using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataProcessor.ImportDto
{
    public class ImportAuthorDto
    {
        [MinLength(Constants.MIN_LENGTH_NAME)]
        [MaxLength(Constants.MAX_LENGTH_NAME)]
        [Required]
        public string FirstName { get; set; }

        [MinLength(Constants.MIN_LENGTH_NAME)]
        [MaxLength(Constants.MAX_LENGTH_NAME)]
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(Constants.PHONE_PATTERN)]
        public string Phone { get; set; }

        public List<ImportBookIdDto> Books { get; set; }

    }
}
