using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(Constants.PartIdLowerCase)]
    public class ImportCarPartDto
    {
        [XmlAttribute(Constants.IdLowerCase)]
        public int PartId { get; set; }
    }
}
