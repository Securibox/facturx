using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Schematron.Helpers;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Securibox.FacturX
{
    public class FacturxSchematronValidator
    {

        public static void ValidateXml(Stream xmlDocumentStream, FacturXConformanceLevelType conformanceLevel)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlDocumentStream);
            ValidateXml(xmlDocument, conformanceLevel);
        }


        public static ValidationResult ValidateXml(XmlDocument xmlDocument, FacturXConformanceLevelType conformanceLevel)
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

        private static Schematron.Types.Schema LoadValidationSchema(FacturXConformanceLevelType conformanceLevel)
        {
            var schemaStream = GetSchemaFileByConformanceLevel(conformanceLevel);
            // check this xmlReaderSettings, is it necessary
            XmlReaderSettings readerSettings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse,
                ValidationType = ValidationType.Schema
            };

            var reader = XmlReader.Create(schemaStream, readerSettings);

            XmlSerializer serializer = new XmlSerializer(typeof(Schematron.Types.Schema));
            Schematron.Types.Schema? schema = serializer.Deserialize(xmlReader: reader) as Schematron.Types.Schema;
            if (schema == null)
            {
                throw new Exception($"Could not load schema for conformance level {conformanceLevel.Id}");
            }

            return schema;
        }

        private static Stream GetSchemaFileByConformanceLevel(FacturXConformanceLevelType conformanceLevel)
        {
            if(conformanceLevel.Id == FacturXConformanceLevelType.Minimum.Id)
            {
                return new MemoryStream(Resources.FACTUR_X_MINIMUM);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.BasicWL.Id)
            {
                return new MemoryStream(Resources.FACTUR_X_BASIC_WL);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.Basic.Id)
            {
                return new MemoryStream(Resources.EN16931_CII_validation_preprocessed);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.EN16931.Id)
            {
                return new MemoryStream(Resources.EN16931_CII_validation_preprocessed);
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.Extended.Id)
            {
                return new MemoryStream(Resources.FACTUR_X_EXTENDED);
            }
            else
            {
                throw new ArgumentException($"Could not determine FacturXConformanceLevelType");
            }
        }


        //public Validator(byte[] schemaByteArray, string facturXProfile)
        //{
        //    MemoryStream schemaStream = new MemoryStream(schemaByteArray);
        //    var reader = XmlReader.Create(schemaStream);
        //    LoadSchema(reader);
        //}

        //private void LoadSchema(XmlReader reader)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(Schematron.Types.Schema));
        //    this.Schema = (Schematron.Types.Schema)serializer.Deserialize(reader);
        //}

        //public ValidationResult ValidateFile(string filename)
        //{
        //    using (Stream stream = File.OpenRead(filename))
        //    {
        //        return ValidateStream(stream);
        //    }
        //}
        //public ValidationResult ValidateXml(XmlDocument xmlDocument)
        //{
        //    using (Stream stream = new MemoryStream())
        //    {
        //        xmlDocument.Save(stream);
        //        stream.Position = 0;
        //        return ValidateStream(stream);
        //    }
        //}

        //public ValidationResult ValidateStream(Stream stream)
        //{
        //    XPathDocument doc = new XPathDocument(stream);

        //    if (this.Schema.Phases != null)
        //    {
        //        var phaseResults = this.Schema.EvaluatePhase(doc.CreateNavigator());
        //        var reportMapper = new ValidationReportMapper(phaseResults);
        //        var report = reportMapper.MapReport();
        //        return report;
        //        //return new ValidationResult(result);
        //    }
        //    else
        //    {
        //        var patternResults = this.Schema.Evaluate(doc.CreateNavigator());

        //        var reportMapper = new ValidationReportMapper(patternResults);
        //        var report = reportMapper.MapReport();
        //        return report;
        //    }
        //}





    }
}
