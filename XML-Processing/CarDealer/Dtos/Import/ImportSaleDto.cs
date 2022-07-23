using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(Constants.Sale)]
    public class ImportSaleDto
    {
        [XmlElement(Constants.CarIdLowerCase)]
        public int CarId { get; set; }

        [XmlElement(Constants.CustomerIdLowerCase)]
        public int CustomerId { get; set; }

        [XmlElement(Constants.DiscountLowerCase)]
        public decimal Discount { get; set; }
    }
}
