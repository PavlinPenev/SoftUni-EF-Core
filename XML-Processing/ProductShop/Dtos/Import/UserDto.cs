using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType(Constants.User)]
    public class UserDto
    {
        [XmlElement(Constants.FirstNameLowerCase)]
        public string FirstName { get; set; }

        [XmlElement(Constants.LastNameLowerCase)]
        public string LastName { get; set; }

        [XmlElement(Constants.AgeXml)]
        public int Age { get; set; }
    }
}
