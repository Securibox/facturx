using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    // title = element title { (text | dir)* }
    public class Title : IXmlSerializable
    {
        public string Value { get; set; }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            this.Value = reader.ReadInnerXml();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("title", this.Value);
        }
    }
}
