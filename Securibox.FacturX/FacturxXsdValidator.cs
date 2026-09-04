using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Securibox.FacturX.Models.Enums;

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
            var asm = Assembly.GetExecutingAssembly();
            var resourcePrefix =
                $"{asm.GetName().Name}.Xsd.FacturX.{conformanceLevel.Name.Replace(' ', '_')}.";

            var resourceNames = asm.GetManifestResourceNames()
                .Where(r =>
                    r.StartsWith(resourcePrefix, StringComparison.OrdinalIgnoreCase)
                    && r.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase)
                );

            foreach (var resourceName in resourceNames)
            {
                using var stream =
                    asm.GetManifestResourceStream(resourceName)
                    ?? throw new FileNotFoundException($"Resource not found: {resourceName}");

                var schema = XmlSchema.Read(stream, null);
                xmlDocument.Schemas.Add(schema);
            }

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
