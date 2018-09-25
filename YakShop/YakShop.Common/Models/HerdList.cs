using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace YakShop.Common.Models
{
    [DataContract(Name ="herd")]
    [XmlRoot("herd")]
    public class HerdList
    {
        [DataMember(Name = "labyak")]
        [XmlElement("labyak")]
        public List<LabYak> Herds { get; set; }
    }

    public class HerdDataList
    {
        public HerdDataList()
        {
            Herd = new List<Herd>();
        }
        [DataMember(Name = "herd")]
        [XmlElement("herd")]
        public List<Herd> Herd { get; set; }
    }

    public class LabYak
    {
        [DataMember(Name = "id")]
        [XmlAttribute("id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [DataMember(Name = "age")]
        [XmlAttribute("age")]
        public decimal Age { get; set; }

        [DataMember(Name = "sex")]
        [XmlAttribute("sex")]
        public string Sex { get; set; }
    }

    public class Herd
    {
        [DataMember(Name = "id")]
        [XmlAttribute("id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        [DataMember(Name = "age")]
        [XmlAttribute("age")]
        public decimal Age { get; set; }

        [DataMember(Name = "age-last-shaved")]
        [XmlAttribute("age-last-shaved")]
        public decimal AgeLastShaved { get; set; }
    }
}
