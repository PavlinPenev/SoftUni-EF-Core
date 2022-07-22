using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType(Constants.Product)]
    public class ExportProductInRangeDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.PriceLowerCase)]
        public decimal Price { get; set; }

        [XmlElement(Constants.BuyerLowerCase)]
        public string BuyerFullName { get; set; }
    }
}
