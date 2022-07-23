using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(Constants.Part)]
    public class ImportPartDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.QuantityLowerCase)]
        public int Quantity { get; set; }

        [XmlElement(Constants.PriceLowerCase)]
        public decimal Price { get; set; }

        [XmlElement(Constants.SupplierIdLowerCase)]
        public int SupplierId { get; set; }
    }
}
