using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType(Constants.CustomerLowerCase)]
    public class ExportCustomerTotalSalesDto
    {
        [XmlAttribute(Constants.FullNameKebabCase)]
        public string FullName { get; set; }

        [XmlAttribute(Constants.BoughtCarsKebabCase)]
        public int BoughtCars { get; set; }

        [XmlAttribute(Constants.SpentMoneyKebabCase)]
        public decimal SpentMoney { get; set; }
    }
}
