using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    /* let = element let {
     *   attribute name { nameValue },
     *   (attribute value { string }
     *   | foreign-element+)
     * }
     */
    [Serializable]
    public class Let : IXmlSerializable
    {
        public string Name { get; set; }

        public bool ValueIsElement { get; set; } = false;

        public string Value { get; set; }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            this.Name = reader.GetAttribute("name");
            if (reader.GetAttribute("value") != null)
            {
                this.Value = reader.GetAttribute("value");
            }
            else
            {
                this.ValueIsElement = true;
                this.Value = reader.ReadInnerXml();
            }
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
