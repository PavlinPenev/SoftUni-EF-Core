using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType(Constants.CarLowerCase)]
    public class ExportCarWithDistanceDto
    {
        [XmlElement(Constants.MakeLowerCase)]
        public string Make { get; set; }

        [XmlElement(Constants.ModelLowerCase)]
        public string Model { get; set; }

        [XmlElement(Constants.TravelledDistanceKebabCase)]
        public long TravelledDistance { get; set; }
    }
}
