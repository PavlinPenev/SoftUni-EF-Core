using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ExportSoldProductsDto
    {
        [XmlElement(Constants.CountLowerCase)]
        public int Count { get; set; }

        [XmlArray(Constants.ProductsLowerCase)]
        public List<ExportProductDtoV2> Products { get; set; }
    }
}
