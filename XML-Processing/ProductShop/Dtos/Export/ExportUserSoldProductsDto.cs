using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType(Constants.User)]
    public class ExportUserSoldProductsDto
    {
        [XmlElement(Constants.FirstNameLowerCase)]
        public string FirstName { get; set;}

        [XmlElement(Constants.LastNameLowerCase)]
        public string LastName { get; set;}

        [XmlArray(Constants.SoldProductsLowerCase)]
        public List<ExportProductDto> SoldProducts { get; set;}
    }
}
