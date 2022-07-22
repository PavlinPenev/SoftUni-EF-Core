using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType(Constants.Product)]
    public class ExportProductDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.PriceLowerCase)]
        public string Price { get; set; }
    }
}
