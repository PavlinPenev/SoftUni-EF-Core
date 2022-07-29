using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType(Constants.BOOK)]
    public class ExportBookDto
    {
        [XmlAttribute(Constants.PAGES)]
        public int Pages { get; set; }

        [XmlElement(Constants.NAME)]
        public string Name { get; set; }

        [XmlElement(Constants.DATE)]
        public string Date { get; set; }
    }
}
