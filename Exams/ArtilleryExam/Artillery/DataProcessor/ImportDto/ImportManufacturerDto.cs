using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType(Constants.MANUFACTURER)]
    public class ImportManufacturerDto
    {
        [XmlElement(Constants.MANUFACTURER_NAME)]
        public string ManufacturerName { get; set; }

        [XmlElement(Constants.FOUNDED)]
        public string Founded { get; set; }
    }
}
