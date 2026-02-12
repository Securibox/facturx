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
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlDocumentStream);
            return ValidateXml(xmlDocument, conformanceLevel);
        }

        public static ValidationResult ValidateXml(
            XmlDocument xmlDocument,
            FacturXConformanceLevelType conformanceLevel
        )
        {
            var validationSchema = LoadValidationSchema(conformanceLevel);

            XmlReader xmlReader = new XmlNodeReader(xmlDocument);
            XPathDocument doc = new XPathDocument(xmlReader);

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
            var schemaStream = GetSchemaFileByConformanceLevel(conformanceLevel);
            // check this xmlReaderSettings, is it necessary
            XmlReaderSettings readerSettings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse,
                ValidationType = ValidationType.Schema,
            };

            var reader = XmlReader.Create(schemaStream, readerSettings);

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

        private static Stream GetSchemaFileByConformanceLevel(
            FacturXConformanceLevelType conformanceLevel
        )
        {
            if (conformanceLevel.Id == FacturXConformanceLevelType.Minimum.Id)
            {
                return new MemoryStream(Resources.Factur_X_1_08_MINIMUM);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.BasicWL.Id)
            {
                return new MemoryStream(Resources.Factur_X_1_08_BASICWL);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.Basic.Id)
            {
                return new MemoryStream(Resources.Factur_X_1_08_BASIC);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.EN16931.Id)
            {
                return new MemoryStream(Resources.Factur_X_1_08_EN16931);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.Extended.Id)
            {
                return new MemoryStream(Resources.Factur_X_1_08_EXTENDED);
            }
            else
            {
                throw new ArgumentException($"Could not determine FacturXConformanceLevelType");
            }
        }
    }
}
