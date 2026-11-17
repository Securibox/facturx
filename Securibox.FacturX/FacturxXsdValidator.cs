using System.Xml;
using System.Xml.Schema;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Schematron.Helpers;

namespace Securibox.FacturX
{
    public class FacturxXsdValidator
    {
        public static bool ValidateXml(
            Stream xmlDocumentStream,
            FacturXConformanceLevelType conformanceLevel,
            List<string> validationErrors
        )
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlDocumentStream);
            return ValidateXml(xmlDocument, conformanceLevel, validationErrors);
        }

        public static bool ValidateXml(
            XmlDocument xmlDocument,
            FacturXConformanceLevelType conformanceLevel,
            List<string> validationErrors
        )
        {
            var schemaSet = new XmlSchemaSet();
            foreach (var xsd in CompressedResources.GetXsdFiles(conformanceLevel))
            {
                using var stream = new MemoryStream(xsd, writable: false);
                var schema = XmlSchema.Read(stream, null);
                schemaSet.Add(schema);
            }
            xmlDocument.Schemas = schemaSet;

            xmlDocument.Validate(
                (sender, eventArgs) => ValidationEventHandler(sender, eventArgs, validationErrors)
            );

            return validationErrors.Count == 0;
        }

        private static void ValidationEventHandler(
            object? sender,
            ValidationEventArgs e,
            List<string> validationErrors
        )
        {
            var message = $"{e.Severity}: {e.Message}";

            validationErrors.Add(message);
        }
    }
}
