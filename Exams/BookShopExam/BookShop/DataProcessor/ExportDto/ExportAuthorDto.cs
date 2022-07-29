using System.Collections.Generic;
namespace BookShop.DataProcessor.ExportDto
{
    public class ExportAuthorDto
    {
        public ExportAuthorDto()
        {
            Books = new List<ExportBookWithPriceDto>();
        }

        public string AuthorName { get; set; }

        public List<ExportBookWithPriceDto> Books { get; set; }
    }
}
