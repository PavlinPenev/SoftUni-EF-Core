using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType(Constants.COUNTRY)]
    public class ExportCountryDto
    {
        [XmlAttribute(Constants.COUNTRY)]
        public string CountryName { get; set; }

        [XmlAttribute(Constants.ARMY_SIZE)]
        public int ArmySize { get; set; }
    }
}
