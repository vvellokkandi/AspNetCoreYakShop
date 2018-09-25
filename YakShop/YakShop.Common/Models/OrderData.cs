using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace YakShop.Common.Models
{
    public class CartData
    {
        [Required]
        [DataMember(Name = "customer")]
        [XmlElement("customer")]
        public string Customer { get; set; }

        [DataMember(Name = "order")]
        [XmlElement("order")]
        public CartItem Order { get; set; }
    }

    public class CartItem
    {
        [JsonIgnore]
        [DataMember(Name = "id")]
        [XmlAttribute("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "milk")]
        [XmlAttribute("milk")]
        public decimal? Milk { get; set; }

        [Required]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "skin")]
        [XmlAttribute("skin")]
        public int? Skin { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public decimal? Milk { get; set; }

        public int? Skin { get; set; }

        public decimal? FullfilledMilk { get; set; }

        public int? FullfilledSkin { get; set; }
    }
}
