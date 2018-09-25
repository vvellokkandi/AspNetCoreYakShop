using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YakShop.Common.Models
{
    public class StockData
    {
        [DataMember(Name = "id")]
        [XmlAttribute("id")]
        public int Id { get; set; }

        [DataMember(Name = "milk")]
        [XmlAttribute("milk")]
        public decimal Milk { get; set; }

        [DataMember(Name = "skin")]
        [XmlAttribute("skin")]
        public int Skin { get; set; }
    }
}
