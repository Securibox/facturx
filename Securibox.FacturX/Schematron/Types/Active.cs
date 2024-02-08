using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    [Serializable]
    public class Active
    {
        [XmlAttribute(AttributeName = "pattern")]
        public string Pattern { get; set; }
    }
}
