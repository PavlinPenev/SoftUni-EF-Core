using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType(Constants.Category)]
    public class ExportCategoryByProductsCountDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.CountLowerCase)]
        public int Count { get; set; }

        [XmlElement(Constants.AveragePriceLowerCase)]
        public decimal AveragePrice { get; set;}

        [XmlElement(Constants.TotalRevenueLowerCase)]
        public decimal TotalRevenue { get; set;}
    }
}
