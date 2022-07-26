using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType(Constants.SaleLowerCase)]
    public class ExportSalesWithAppliedDiscountDto
    {
        [XmlElement(Constants.CarLowerCase)]
        public ExportCarForSalesDto Car { get; set; }

        [XmlElement(Constants.DiscountLowerCase)]
        public decimal Discount { get; set; }

        [XmlElement(Constants.CustomerNameKebabCase)]
        public string CustomerName { get; set; }

        [XmlElement(Constants.PriceLowerCase)]
        public decimal Price { get; set; }

        [XmlElement(Constants.PriceWithDiscountKebabCase)]
        public decimal PriceWithDiscount { get; set; }
    }
}
