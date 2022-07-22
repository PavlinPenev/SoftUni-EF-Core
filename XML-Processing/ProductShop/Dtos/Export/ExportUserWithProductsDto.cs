using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType(Constants.User)]
    public class ExportUserWithProductsDto
    {
        [XmlElement(Constants.FirstNameLowerCase)]
        public string FirstName { get; set;}

        [XmlElement(Constants.LastNameLowerCase)]
        public string LastName { get; set;}

        [XmlElement(Constants.AgeXml)]
        public int? Age { get; set;}

        [XmlElement(Constants.SoldProducts)]
        public ExportSoldProductsDto SoldProducts { get; set; }
    }
}
