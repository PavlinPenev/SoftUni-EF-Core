using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType(Constants.SuplierLowerCase)]
    public class ExportLocalSupplierDto
    {
        [XmlAttribute(Constants.IdLowerCase)]
        public int Id { get; set; }

        [XmlAttribute(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlAttribute(Constants.PartsCountKebabCase)]
        public int PartsCount { get; set; }
    }
}
