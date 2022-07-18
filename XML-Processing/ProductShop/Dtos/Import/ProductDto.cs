using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType(Constants.Product)]
    public class ProductDto
    {
        [XmlElement(Constants.NameLowerCase)]
        public string Name { get; set; }

        [XmlElement(Constants.PriceLowerCase)]
        public decimal Price { get; set; }

        [XmlElement(Constants.BuyerIdLowerCase)]
        public int? BuyerId { get; set; }


        [XmlElement(Constants.SellerIdLowerCase)]
        public int SellerId { get; set; }
    }
}
