using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class BinaryObject
    {
        [XmlAttribute("mimeCode")]
        public string MimeCode { get; set; }

        [XmlAttribute("filename")]
        public string Filename { get; set; }
    }
}