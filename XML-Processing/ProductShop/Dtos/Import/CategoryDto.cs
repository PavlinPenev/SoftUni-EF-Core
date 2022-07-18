using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType(Constants.Category)]
    public class CategoryDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }
    }
}
