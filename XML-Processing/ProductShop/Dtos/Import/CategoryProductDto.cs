using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType(Constants.CategoryProduct)]
    public class CategoryProductDto
    {
        [XmlElement(Constants.CategoryId)]
        public int CategoryId { get; set; }

        [XmlElement(Constants.ProductId)]
        public int ProductId { get; set; }
    }
}
