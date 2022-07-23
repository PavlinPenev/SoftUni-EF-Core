using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(Constants.Supplier)]
    public class ImportSupplierDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.IsImporterLowerCase)]
        public bool IsImporter { get; set; }
    }
}
