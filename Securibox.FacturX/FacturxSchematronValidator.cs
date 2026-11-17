using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Schematron.Helpers;

namespace Securibox.FacturX
{
    public class FacturxSchematronValidator
    {
        public static ValidationResult ValidateXml(
            Stream xmlDocumentStream,
            FacturXConformanceLevelType conformanceLevel
        )
        {
            var validationSchema = LoadValidationSchema(conformanceLevel);

            var xmlReader = new XmlTextReader(xmlDocumentStream);
            var doc = new XPathDocument(xmlReader);

            if (validationSchema.Phases != null)
            {
                var phaseResults = validationSchema.EvaluatePhase(doc.CreateNavigator());
                var reportMapper = new ValidationReportMapper(phaseResults);
                var report = reportMapper.MapReport();
                return report;
            }
            else
            {
                var patternResults = validationSchema.Evaluate(doc.CreateNavigator());
                var reportMapper = new ValidationReportMapper(patternResults);
                var report = reportMapper.MapReport();
                return report;
            }
        }

        private static Schematron.Types.Schema LoadValidationSchema(
            FacturXConformanceLevelType conformanceLevel
        )
        {
            using var schemaStream = new MemoryStream(
                CompressedResources.GetSchema(conformanceLevel),
                writable: false
            );
            // check this xmlReaderSettings, is it necessary
            XmlReaderSettings readerSettings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse,
                ValidationType = ValidationType.Schema,
            };

            using var reader = XmlReader.Create(schemaStream, readerSettings);

            XmlSerializer serializer = new XmlSerializer(typeof(Schematron.Types.Schema));
            Schematron.Types.Schema? schema =
                serializer.Deserialize(xmlReader: reader) as Schematron.Types.Schema;
            if (schema == null)
            {
                throw new Exception(
                    $"Could not load schema for conformance level {conformanceLevel.Id}"
                );
            }

            return schema;
        }
    }
}
