using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(Constants.Car)]
    public class ImportCarDto
    {
        [XmlElement(Constants.MakeLowerCase)]
        public string Make { get; set; }

        [XmlElement(Constants.ModelLowerCase)]
        public string Model { get; set; }

        [XmlElement(Constants.TravelledDistance)]
        public long TravelledDistance { get; set; }

        [XmlArray(Constants.PartsLowerCase)]
        public List<ImportCarPartDto> PartsIds { get; set; }
    }
}
