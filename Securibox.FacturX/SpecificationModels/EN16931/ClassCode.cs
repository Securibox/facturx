using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class ClassCode
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute("@listID")]
        public string ListID { get; set; }

        [XmlAttribute("@listVersionID")]
        public string ListVersionID { get; set; }
    }
}
