using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class ID
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute("schemeID")]
        public string SchemeID { get; set; }
    }
}
