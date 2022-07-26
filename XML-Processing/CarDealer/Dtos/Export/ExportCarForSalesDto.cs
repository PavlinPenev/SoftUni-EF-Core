using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType(Constants.CarLowerCase)]
    public class ExportCarForSalesDto
    {
        [XmlAttribute(Constants.MakeLowerCase)]
        public string Make { get; set; }

        [XmlAttribute(Constants.ModelLowerCase)]
        public string Model { get; set; }

        [XmlAttribute(Constants.TravelledDistanceKebabCase)]
        public long TravelledDistance { get; set; }
    }
}
