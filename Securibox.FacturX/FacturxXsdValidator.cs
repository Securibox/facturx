using Securibox.FacturX.Models.Enums;
using System.Reflection;
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
            var asm = Assembly.GetExecutingAssembly();
            var resourcePrefix = $"{asm.GetName().Name}.Xsd.FacturX.{conformanceLevel.Name}.";

            var resourceNames = asm.GetManifestResourceNames()
                                   .Where(r => r.StartsWith(resourcePrefix, StringComparison.OrdinalIgnoreCase)
                                               && r.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase));

            foreach (var resourceName in resourceNames)
            {
                using var stream = asm.GetManifestResourceStream(resourceName)
                                   ?? throw new FileNotFoundException($"Resource not found: {resourceName}");

                var schema = XmlSchema.Read(stream, null);
                xmlDocument.Schemas.Add(schema);
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
