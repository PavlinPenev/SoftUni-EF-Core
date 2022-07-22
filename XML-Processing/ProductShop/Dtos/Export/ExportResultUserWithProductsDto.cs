using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ExportResultUserWithProductsDto
    {
        [XmlElement(Constants.CountLowerCase)]
        public int Count { get; set; }

        [XmlArray(Constants.UsersLowerCase)]
        public List<ExportUserWithProductsDto> Users { get; set; }
    }
}
