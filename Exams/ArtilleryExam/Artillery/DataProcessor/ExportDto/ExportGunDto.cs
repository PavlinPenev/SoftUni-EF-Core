using System.Collections.Generic;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType(Constants.GUN)]
    public class ExportGunDto
    {
        [XmlAttribute(Constants.MANUFACTURER)]
        public string Manufacturer { get; set; }

        [XmlAttribute(Constants.GUN_TYPE)]
        public string GunType { get; set;}

        [XmlAttribute(Constants.GUN_WEIGHT)]
        public int GunWeight { get; set;}

        [XmlAttribute(Constants.BARREL_LENGTH)]
        public double BarrelLength { get; set;}

        [XmlAttribute(Constants.RANGE)]
        public int Range { get; set;}

        [XmlArray(Constants.COUNTRIES)]
        public List<ExportCountryDto> Countries { get; set; }
    }
}
