using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    /*
     * p = element p {
     *   attribute id { xsd:ID }?,
     *   attribute class { classValue }?,
     *   attribute icon { uriValue }?,
     *   (foreign & (text | dir | emph | span)*)
     * }
     */
    [Serializable]
    public class P : IXmlSerializable
    {
        public string Id { get; set; }

        public string Class { get; set; }

        public string Icon { get; set; }

        public string FormattedText { get; set; }
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            this.Id = reader.GetAttribute("id");
            this.Class = reader.GetAttribute("class");
            this.Icon = reader.GetAttribute("icon");
            this.FormattedText = reader.ReadInnerXml();
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
