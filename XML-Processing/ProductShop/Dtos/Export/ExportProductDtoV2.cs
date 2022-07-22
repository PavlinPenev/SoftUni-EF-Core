using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType(Constants.Product)]
    public class ExportProductDtoV2
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.PriceLowerCase)]
        public decimal Price { get; set; }
    }
}
