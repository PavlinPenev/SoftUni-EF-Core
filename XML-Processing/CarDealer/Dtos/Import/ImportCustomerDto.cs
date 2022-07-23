using System;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(Constants.Customer)]
    public class ImportCustomerDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.BirthdateLowerCase)]
        public DateTime Birthdate { get; set;}

        [XmlElement(Constants.IsYoungDriverLowerCase)]
        public bool IsYoungDriver { get; set;}
    }
}
