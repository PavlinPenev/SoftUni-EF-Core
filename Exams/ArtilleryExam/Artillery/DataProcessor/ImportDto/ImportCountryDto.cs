using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType(Constants.COUNTRY)]
    public class ImportCountryDto
    {
        [XmlElement(Constants.COUNTRY_NAME)]
        public string CountryName { get; set; }

        [XmlElement(Constants.ARMY_SIZE)]
        public int ArmySize { get; set; }
    }
}
