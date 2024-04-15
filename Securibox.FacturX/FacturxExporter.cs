using Microsoft.Extensions.Logging;
using System.Text;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.Security.Cryptography;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Minimum;
using Securibox.FacturX.SpecificationModels;
using System.Xml.Serialization;
using System.Xml;

namespace Securibox.FacturX
{
    public class FacturxExporter
    {
        private ILogger<FacturxExporter> _logger;

        public FacturxExporter(ILogger<FacturxExporter>? logger = null)
        {
            InitializeLogger(logger);

        }

        public Stream CreateFacturXStream(string pdfPath, string xmlPath, FacturXConformanceLevelType conformanceLevel, string documentTitle = "Invoice", string documentDescription = "Invoice description")
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }

            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("File not found", xmlPath);
            }
            return CreateFacturXStream(File.OpenRead(pdfPath), File.OpenRead(xmlPath), conformanceLevel, documentTitle, documentDescription);
        }

        public Stream CreateFacturXStream(string pdfPath, Invoice invoice, string documentTitle = "Invoice", string documentDescription = "Invoice description")
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice));
            }

            var xmlPath = @"C:\Temp\FacturX\New folder\Lavarin\2023-6013 - Jappera - BASIC.xml";


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
            return CreateFacturXStream(File.OpenRead(pdfPath), File.OpenRead(xmlPath), conformanceLevel, documentTitle, documentDescription);
        }

        public Stream CreateFacturXStream(string pdfPath, ICrossIndustryInvoice invoice, string documentTitle = "Invoice", string documentDescription = "Invoice description")
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

                    return CreateFacturXStream(File.OpenRead(pdfPath), stream, conformanceLevel, documentTitle, documentDescription);
                }
            }
        }



        public Stream CreateFacturXStream(Stream pdfStream, Stream xmlStream, FacturXConformanceLevelType conformanceLevel, string documentTitle = "Invoice", string documentDescription = "Invoice description")
        {
            if (pdfStream == null)
            {
                throw new ArgumentNullException(nameof(pdfStream));
            }

            if (xmlStream == null)
            {
                throw new ArgumentNullException(nameof(xmlStream));
            }

            _logger.LogInformation($"Validating XML with XSD validation for conformance level {conformanceLevel.Name}");
            FacturxXsdValidator.ValidateXml(xmlStream, conformanceLevel);

            _logger.LogInformation($"Success in validating XML with XSD validation for conformance level {conformanceLevel.Name}");

            var pdfDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);

            PdfDocument outputDocument = new PdfDocument();
            for (int i = 0; i < pdfDocument.PageCount; i++)
            {
                outputDocument.AddPage(pdfDocument.Pages[i]);
            }

            string xmlChecksum = string.Empty;
            byte[] xmlFileBytes = null;
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(xmlStream);
                xmlChecksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

                xmlStream.Seek(0, SeekOrigin.Begin);
                xmlFileBytes = new byte[xmlStream.Length];
                xmlStream.Read(xmlFileBytes, 0, (int)xmlStream.Length);
            }

            _logger.LogInformation($"Calculated the MD5 checksum for the XML file");

            var xmlFileEncodedBytes = PdfSharpCore.Pdf.Filters.Filtering.FlateDecode.Encode(xmlFileBytes);

            PdfDictionary xmlParamsDict = new PdfDictionary();
            xmlParamsDict.Elements.Add("/CheckSum", new PdfString(xmlChecksum));
            xmlParamsDict.Elements.Add("/ModDate", new PdfString("D:" + DateTime.UtcNow.ToString("yyyyMMddHHmmsszzz")));
            xmlParamsDict.Elements.Add("/Size", new PdfInteger(xmlFileBytes.Length));



            PdfDictionary fStreamDict = new PdfDictionary();
            fStreamDict.CreateStream(xmlFileEncodedBytes);
            fStreamDict.Elements.Add("/Filter", new PdfName("/FlateDecode"));
            fStreamDict.Elements.Add("/Type", new PdfName("/EmbeddedFile"));
            fStreamDict.Elements.Add("/Params", xmlParamsDict);
            fStreamDict.Elements.Add("/Subtype", new PdfName("/text/xml"));
            outputDocument.Internals.AddObject(fStreamDict);


            PdfDictionary af0Dict = new PdfDictionary();
            af0Dict.Elements.Add("/AFRelationship", new PdfName("/Data"));
            af0Dict.Elements.Add("/Desc", new PdfString("Factur-X XML file"));
            af0Dict.Elements.Add("/Type", new PdfName("/Filespec"));
            af0Dict.Elements.Add("/F", new PdfString("factur-x.xml"));
            PdfDictionary af1Dict = new PdfDictionary();
            af1Dict.Elements.Add("/F", fStreamDict.Reference);
            af1Dict.Elements.Add("/UF", fStreamDict.Reference);

            af0Dict.Elements.Add("/EF", af1Dict);
            af0Dict.Elements.Add("/UF", new PdfString("factur-x.xml"));
            outputDocument.Internals.AddObject(af0Dict);

            var afPdfArray = new PdfArray();
            afPdfArray.Elements.Add(af0Dict.Reference);
            outputDocument.Internals.AddObject(afPdfArray);
            outputDocument.Internals.Catalog.Elements.Add("/AF", afPdfArray.Reference);


            var dateTimeNow = DateTime.UtcNow;
            var conformanceLevelName = conformanceLevel.Name.ToUpperInvariant();
            var xmpmeta = Resources.PdfMetadataTemplate
                .Replace("{{CreationDate}}", dateTimeNow.ToString("yyyy-MM-ddThh:mm:sszzz"))
                .Replace("{{ModificationDate}}", dateTimeNow.ToString("yyyy-MM-ddThh:mm:sszzz"))
                .Replace("{{DocumentTitle}}", documentTitle)
                .Replace("{{DocumentDescription}}", documentDescription)
                .Replace("{{ConformanceLevel}}", conformanceLevelName);

            var metadataBytes = System.Text.Encoding.UTF8.GetBytes(xmpmeta);
            var metadataEncodedBytes = PdfSharpCore.Pdf.Filters.Filtering.FlateDecode.Encode(metadataBytes);

            PdfDictionary metadataDictionary = new PdfDictionary();
            metadataDictionary.CreateStream(metadataEncodedBytes);
            metadataDictionary.Elements.Add("/Filter", new PdfName("/FlateDecode"));
            metadataDictionary.Elements.Add("/Subtype", new PdfName("/XML"));
            metadataDictionary.Elements.Add("/Type", new PdfName("/Metadata"));
            outputDocument.Internals.AddObject(metadataDictionary);

            outputDocument.Internals.Catalog.Elements.Add("/Metadata", metadataDictionary.Reference);


            var namesPdfArray = new PdfArray();
            namesPdfArray.Elements.Add(new PdfString("factur-x.xml"));
            namesPdfArray.Elements.Add(af0Dict.Reference);
            PdfDictionary embeddedFilesDict = new PdfDictionary();
            embeddedFilesDict.Elements.Add("/Names", namesPdfArray);
            PdfDictionary namesDict = new PdfDictionary();
            namesDict.Elements.Add("/EmbeddedFiles", embeddedFilesDict);

            outputDocument.Internals.Catalog.Elements.Add("/Names", namesDict);



            PdfDictionary rgbProfileDict = new PdfDictionary();
            rgbProfileDict.CreateStream(Resources.sRGB_IEC61966_2_1);
            rgbProfileDict.Elements.Add("/N", new PdfInteger(3));
            outputDocument.Internals.AddObject(rgbProfileDict);

            PdfDictionary outputIntent0Dict = new PdfDictionary();
            outputIntent0Dict.Elements.Add("/DestOutputProfile", rgbProfileDict.Reference);
            outputIntent0Dict.Elements.Add("/OutputConditionIdentifier", new PdfString("sRGB IEC61966-2.1"));
            outputIntent0Dict.Elements.Add("/S", new PdfName("/GTS_PDFA1"));
            outputIntent0Dict.Elements.Add("/Type", new PdfName("/OutputIntent"));
            outputDocument.Internals.AddObject(outputIntent0Dict);

            var outputIntentsArray = new PdfArray();
            outputIntentsArray.Elements.Add(outputIntent0Dict.Reference);
            outputDocument.Internals.Catalog.Elements.Add("/OutputIntents", outputIntentsArray);

            outputDocument.Info.Creator = "Securibox";

            MemoryStream memoryStream = new MemoryStream();
            outputDocument.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            _logger.LogInformation($"Successfully generated the Factur-X PDF stream.");
            return memoryStream;
        }

        private void InitializeLogger(ILogger<FacturxExporter>? logger)
        {
            if (logger == null)
            {
                using ILoggerFactory factory = LoggerFactory.Create(delegate (ILoggingBuilder builder)
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning).AddFilter("System", LogLevel.Warning).AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
                });

                _logger = factory.CreateLogger<FacturxExporter>();
            }
            else
            {
                _logger = logger;
            }
        }
    }
}
