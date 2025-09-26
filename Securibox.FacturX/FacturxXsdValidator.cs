using Securibox.FacturX.Models.Enums;
using System.Xml;
using System.Xml.Schema;

namespace Securibox.FacturX
{
    public class FacturxXsdValidator
    {
        public static void ValidateXml(Stream xmlDocumentStream, FacturXConformanceLevelType conformanceLevel)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlDocumentStream);
            ValidateXml(xmlDocument, conformanceLevel);
        }

        public static void ValidateXml(XmlDocument xmlDocument, FacturXConformanceLevelType conformanceLevel)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            path = Path.Combine(path, "Xsd", "FacturX", conformanceLevel.Name);
            var XsdDirectory = new DirectoryInfo(path);
            foreach (FileInfo file in XsdDirectory.GetFiles("*.xsd"))
            {
                using (FileStream fileStream = File.OpenRead(file.FullName))
                {
                    XmlSchema schema = XmlSchema.Read(fileStream, null);
                    xmlDocument.Schemas.Add(schema);
                }
            }


            xmlDocument.Validate(ValidationEventHandler!);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }
    }
}
