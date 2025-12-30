using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Minimum;
using Securibox.FacturX.Schematron.Helpers;
using Securibox.FacturX.SpecificationModels;

namespace Securibox.FacturX
{
    public class FacturxExporter
    {
        private ILogger<FacturxExporter> _logger;
        public List<ValidationReport> validationReport;

        public FacturxExporter(ILogger<FacturxExporter>? logger = null)
        {
            InitializeLogger(logger);
            validationReport = [];
        }

        public Stream CreateFacturXStream(string pdfPath, string xmlPath, FacturXConformanceLevelType conformanceLevel, string documentTitle = "Invoice", string documentDescription = "Invoice description", bool failOnInvalid = false)
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }

            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("File not found", xmlPath);
            }

            using var pdfStream = File.OpenRead(pdfPath);
            using var xmlStream = File.OpenRead(xmlPath);
            return CreateFacturXStream(pdfStream, xmlStream, conformanceLevel, documentTitle, documentDescription, failOnInvalid);
        }

        public Stream CreateFacturXStream(string pdfPath, string xmlPath, Invoice invoice, string documentTitle = "Invoice", string documentDescription = "Invoice description", bool failOnInvalid = false)
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice));
            }

            var invoiceType = invoice.GetType();
            var conformanceLevel = FacturXConformanceLevelType.Minimum;
            if (invoiceType == typeof(Models.BasicWL.Invoice))
            {
                conformanceLevel = FacturXConformanceLevelType.BasicWL;
            }
            else if (invoiceType == typeof(Models.Basic.Invoice))
            {
                conformanceLevel = FacturXConformanceLevelType.Basic;
            }
            else if (invoiceType == typeof(Models.EN16931.Invoice))
            {
                conformanceLevel = FacturXConformanceLevelType.EN16931;
            }
            else if (invoiceType == typeof(Models.Extended.Invoice))
            {
                conformanceLevel = FacturXConformanceLevelType.Extended;
            }
            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("File not found", xmlPath);
            }

            using var pdfStream = File.OpenRead(pdfPath);
            using var xmlFileStream = File.OpenRead(xmlPath);
            return CreateFacturXStream(pdfStream, xmlFileStream, conformanceLevel, documentTitle, documentDescription, failOnInvalid);
        }

        public Stream CreateFacturXStream(string pdfPath, ICrossIndustryInvoice invoice, string documentTitle = "Invoice", string documentDescription = "Invoice description", bool failOnInvalid = false)
        {
            ArgumentNullException.ThrowIfNull(invoice);
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }

            var invoiceType = invoice.GetType();
            var conformanceLevel = FacturXConformanceLevelType.Minimum;
            if (invoiceType == typeof(SpecificationModels.BasicWL.CrossIndustryInvoice))
            {
                conformanceLevel = FacturXConformanceLevelType.BasicWL;
            }
            else if (invoiceType == typeof(SpecificationModels.Basic.CrossIndustryInvoice))
            {
                conformanceLevel = FacturXConformanceLevelType.Basic;
            }
            else if (invoiceType == typeof(SpecificationModels.EN16931.CrossIndustryInvoice))
            {
                conformanceLevel = FacturXConformanceLevelType.EN16931;
            }
            else if (invoiceType == typeof(SpecificationModels.Extended.CrossIndustryInvoice))
            {
                conformanceLevel = FacturXConformanceLevelType.Extended;
            }

            var serializer = new XmlSerializer(invoiceType);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            namespaces.Add("ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            namespaces.Add("rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            namespaces.Add("udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("", "");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding();
            settings.Indent = true;

            //create an XmlWriter that utilizes a StringWriter to
            //build the output, then write that to the Console window
            using (Stream stream = new MemoryStream())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(xmlWriter, invoice, namespaces);
                    stream.Seek(0, SeekOrigin.Begin);

                    using var pdfStream = File.OpenRead(pdfPath);
                    return CreateFacturXStream(pdfStream, stream, conformanceLevel, documentTitle, documentDescription, failOnInvalid);
                }
            }
        }

        public Stream CreateFacturXStream(Stream pdfStream, Stream xmlStream, FacturXConformanceLevelType conformanceLevel, string documentTitle = "Invoice", string documentDescription = "Invoice description", bool failOnInvalid = false)
        {
            ArgumentNullException.ThrowIfNull(pdfStream);
            ArgumentNullException.ThrowIfNull(xmlStream);

            _logger.LogInformation($"Validating XML for {conformanceLevel.Name}");
            FacturxXsdValidator.ValidateXml(xmlStream, conformanceLevel);

            bool isNotValid = false;
            xmlStream.Position = 0;
            var schValidationResult = FacturxSchematronValidator.ValidateXml(xmlStream, conformanceLevel);
            if (!schValidationResult._isSuccessfullValidation)
            {
                var errors = schValidationResult._results.Where(x => x.IsError == true || x.IsWarning == true).ToList();
                for (int i = 0; i < errors.Count; i++)
                {
                    if (!isNotValid && errors[i].IsError)
                    {
                        isNotValid = true;
                    }

                    validationReport.Add(errors[i]);
                }
            }

            if (failOnInvalid && isNotValid)
            {
                throw new Exception("The provided XML is not valid according to the selected Factur-X conformance level. " + string.Join(Environment.NewLine, validationReport.Select(r => $"Path: {r.Path}, Test: {r.Test}, Description: {r.Description}, BusinessRuleCode: {r.BusinessRuleCode}, " + $"ContextLine: {r.ContextLine}, ContextPosition: {r.ContextPosition}, ContextElement: {r.ContextElement}, " + $"IsError: {r.IsError}, IsWarning: {r.IsWarning}")));
            }

            _logger.LogInformation($"Success in validating XML with XSD validation for conformance level {conformanceLevel.Name}");

            Stream inputPdfStream = pdfStream;
            MemoryStream? tempStream = null;

            if (!pdfStream.CanSeek)
            {
                tempStream = new MemoryStream();
                pdfStream.CopyTo(tempStream);
                tempStream.Position = 0;
                inputPdfStream = tempStream;
            }

            try
            {
                using var inputDocument = PdfReader.Open(inputPdfStream, PdfDocumentOpenMode.Import);
                var outputDocument = new PdfDocument();

                foreach (var page in inputDocument.Pages)
                {
                    outputDocument.AddPage(page);
                }

                outputDocument.Version = 17;

                string xmlChecksum;
                byte[] xmlFileBytes;
                using (var md5 = MD5.Create())
                {
                    var hashBytes = md5.ComputeHash(xmlStream);
                    xmlChecksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                    xmlStream.Position = 0;
                    xmlFileBytes = new byte[xmlStream.Length];
                    int bytesRead = 0;
                    while (bytesRead < xmlFileBytes.Length)
                    {
                        int read = xmlStream.Read(xmlFileBytes, bytesRead, xmlFileBytes.Length - bytesRead);
                        if (read == 0) break;
                        bytesRead += read;
                    }
                }

                PdfDictionary fStreamDict = new PdfDictionary(outputDocument);
                fStreamDict.CreateStream(xmlFileBytes);
                fStreamDict.Elements.Remove("/Filter");

                PdfDictionary xmlParamsDict = new PdfDictionary(outputDocument);
                xmlParamsDict.Elements["/CheckSum"] = new PdfString(xmlChecksum);
                xmlParamsDict.Elements["/ModDate"] = new PdfString("D:" + DateTime.UtcNow.ToString("yyyyMMddHHmmsszzz"));
                xmlParamsDict.Elements["/Size"] = new PdfInteger(xmlFileBytes.Length);

                fStreamDict.Elements["/Type"] = new PdfName("/EmbeddedFile");
                fStreamDict.Elements["/Params"] = xmlParamsDict;
                fStreamDict.Elements["/Subtype"] = new PdfName("/text/xml");

                outputDocument.Internals.AddObject(fStreamDict);

                PdfDictionary af0Dict = new PdfDictionary(outputDocument);
                af0Dict.Elements["/AFRelationship"] = new PdfName("/Data");
                af0Dict.Elements["/Desc"] = new PdfString("Factur-X XML file");
                af0Dict.Elements["/Type"] = new PdfName("/Filespec");
                af0Dict.Elements["/F"] = new PdfString("factur-x.xml");
                af0Dict.Elements["/UF"] = new PdfString("factur-x.xml");

                PdfDictionary af1Dict = new PdfDictionary(outputDocument);
                af1Dict.Elements["/F"] = fStreamDict.Reference;
                af1Dict.Elements["/UF"] = fStreamDict.Reference;

                af0Dict.Elements["/EF"] = af1Dict;
                outputDocument.Internals.AddObject(af0Dict);

                var afPdfArray = new PdfArray(outputDocument);
                afPdfArray.Elements.Add(af0Dict.Reference);
                outputDocument.Internals.AddObject(afPdfArray);
                outputDocument.Internals.Catalog.Elements["/AF"] = afPdfArray.Reference;

                var dateTimeNow = DateTime.UtcNow;
                var xmpmeta = Resources.PdfMetadataTemplate.Replace("{{CreationDate}}", dateTimeNow.ToString("yyyy-MM-ddTHH:mm:sszzz")).Replace("{{ModificationDate}}", dateTimeNow.ToString("yyyy-MM-ddTHH:mm:sszzz")).Replace("{{DocumentTitle}}", documentTitle).Replace("{{DocumentDescription}}", documentDescription).Replace("{{ConformanceLevel}}", conformanceLevel.Name.ToUpperInvariant());

                xmpmeta += new string(' ', 2000);
                var metadataBytes = new UTF8Encoding(false).GetBytes(xmpmeta);

                PdfDictionary metadataDictionary = new PdfDictionary(outputDocument);
                metadataDictionary.CreateStream(metadataBytes);
                metadataDictionary.Elements["/Type"] = new PdfName("/Metadata");
                metadataDictionary.Elements["/Subtype"] = new PdfName("/XML");
                metadataDictionary.Elements.Remove("/Filter");

                outputDocument.Internals.AddObject(metadataDictionary);
                outputDocument.Internals.Catalog.Elements["/Metadata"] = metadataDictionary.Reference;

                var namesPdfArray = new PdfArray(outputDocument);
                namesPdfArray.Elements.Add(new PdfString("factur-x.xml"));
                namesPdfArray.Elements.Add(af0Dict.Reference);

                PdfDictionary embeddedFilesDict = new PdfDictionary(outputDocument);
                embeddedFilesDict.Elements["/Names"] = namesPdfArray;

                PdfDictionary namesDict = new PdfDictionary(outputDocument);
                namesDict.Elements["/EmbeddedFiles"] = embeddedFilesDict;
                outputDocument.Internals.Catalog.Elements["/Names"] = namesDict;

                PdfDictionary rgbProfileDict = new PdfDictionary(outputDocument);
                rgbProfileDict.CreateStream(Resources.sRGB_IEC61966_2_1);
                rgbProfileDict.Elements["/N"] = new PdfInteger(3);
                rgbProfileDict.Elements.Remove("/Filter");

                outputDocument.Internals.AddObject(rgbProfileDict);

                PdfDictionary outputIntent0Dict = new PdfDictionary(outputDocument);
                outputIntent0Dict.Elements["/DestOutputProfile"] = rgbProfileDict.Reference;
                outputIntent0Dict.Elements["/OutputConditionIdentifier"] = new PdfString("sRGB IEC61966-2.1");
                outputIntent0Dict.Elements["/S"] = new PdfName("/GTS_PDFA1");
                outputIntent0Dict.Elements["/Type"] = new PdfName("/OutputIntent");
                outputDocument.Internals.AddObject(outputIntent0Dict);

                var outputIntentsArray = new PdfArray(outputDocument);
                outputIntentsArray.Elements.Add(outputIntent0Dict.Reference);
                outputDocument.Internals.Catalog.Elements["/OutputIntents"] = outputIntentsArray;
                outputDocument.Info.Creator = "Securibox";
                MemoryStream memoryStream = new MemoryStream();
                outputDocument.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"Successfully generated the Factur-X PDF stream.");
                return memoryStream;
            }
            finally
            {
                tempStream?.Dispose();
            }
        }

        private void InitializeLogger(ILogger<FacturxExporter>? logger)
        {
            if (logger == null)
            {
                using ILoggerFactory factory = LoggerFactory.Create(
                    delegate(ILoggingBuilder builder)
                    {
                        builder.AddFilter("Microsoft", LogLevel.Warning).AddFilter("System", LogLevel.Warning).AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
                    }
                );

                _logger = factory.CreateLogger<FacturxExporter>();
            }
            else
            {
                _logger = logger;
            }
        }
    }
}