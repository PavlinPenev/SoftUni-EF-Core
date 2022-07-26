using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType(Constants.PartLowerCase)]
    public class ExportPartDto
    {
        [XmlAttribute(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlAttribute(Constants.PriceLowerCase)]
        public decimal Price { get; set; }
    }
}
