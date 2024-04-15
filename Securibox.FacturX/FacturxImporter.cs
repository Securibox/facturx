using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;
using Securibox.FacturX.Core;
using Securibox.FacturX.Models;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Minimum;
using Securibox.FacturX.SpecificationModels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using XmpCore;
using static PdfSharpCore.Pdf.PdfDictionary;

namespace Securibox.FacturX
{
    public class FacturxImporter
    {
        private ILogger<FacturxImporter> _logger;
        private XmlDocument _xmlDocument;
        private FacturXMetadata _facturXMetadata;
        private PdfDocument _pdfDocument;

        public FacturxImporter(Stream pdfStream, ILogger<FacturxImporter>? logger = null)
        {
            InitializeLogger(logger);
            _pdfDocument = PdfReader.Open(pdfStream);
        }

        public FacturxImporter(string pdfFilename, ILogger<FacturxImporter>? logger = null)
        {
            InitializeLogger(logger);
            if (!File.Exists(pdfFilename))
            {
                throw new FileNotFoundException("File not found", pdfFilename);
            }

            using (var pdfFile = File.OpenRead(pdfFilename))
            {
                _pdfDocument = PdfReader.Open(pdfFile);
            }
        }

        public FacturXMetadata GetMetadata()
        {
            if (_facturXMetadata == null)
            {
                _facturXMetadata = GetFacturXMetadata(_pdfDocument);
            }

            return _facturXMetadata;
        }

        public bool IsFacturXValid()
        {
            var facturXMetadata = GetMetadata();
            var xmlPdfStream = GetEmbeddedXmlStream(_pdfDocument, facturXMetadata.DocumentFileName);
            if (xmlPdfStream == null || xmlPdfStream.Length == 0)
            {
                _logger!.LogCritical("Could not read embedded XML file.");
                throw new IOException("Could not read embedded XML file.");
            }
            LoadXml(xmlPdfStream);

            FacturxXsdValidator.ValidateXml(_xmlDocument, facturXMetadata.ConformanceLevel);
            var validationResult = FacturxSchematronValidator.ValidateXml(_xmlDocument, facturXMetadata.ConformanceLevel);
            if (!validationResult._isSuccessfullValidation)
            {
                throw new Exception("Invalid Xml.");
            }
            return true;
        }

        public ICrossIndustryInvoice ImportDataWithDeserialization()
        {
            IsFacturXValid();

            if (_facturXMetadata.ConformanceLevel == FacturXConformanceLevelType.Minimum)
            {
                return this.Deserialize<SpecificationModels.Minimum.CrossIndustryInvoice>(_xmlDocument);
            }
            else if (_facturXMetadata.ConformanceLevel == FacturXConformanceLevelType.BasicWL)
            {
                return this.Deserialize<SpecificationModels.BasicWL.CrossIndustryInvoice>(_xmlDocument);
            }
            else if (_facturXMetadata.ConformanceLevel == FacturXConformanceLevelType.Basic)
            {
                return this.Deserialize<SpecificationModels.Basic.CrossIndustryInvoice>(_xmlDocument);
            }
            else if (_facturXMetadata.ConformanceLevel == FacturXConformanceLevelType.EN16931)
            {
                return this.Deserialize<SpecificationModels.EN16931.CrossIndustryInvoice>(_xmlDocument);
            }
            else if (_facturXMetadata.ConformanceLevel == FacturXConformanceLevelType.Extended)
            {
                return this.Deserialize<SpecificationModels.Extended.CrossIndustryInvoice>(_xmlDocument);
            }
            return null;
        }

        private T Deserialize<T>(XmlDocument document)
           where T : class
        {
            XmlReader reader = new XmlNodeReader(document);
            var serializer = new XmlSerializer(typeof(T));
            T result = (T)serializer.Deserialize(reader);
            return result;
        }

        private void InitializeLogger(ILogger<FacturxImporter> logger)
        {
            if (logger == null)
            {
                using ILoggerFactory factory = LoggerFactory.Create(delegate (ILoggingBuilder builder)
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning).AddFilter("System", LogLevel.Warning).AddFilter(nameof(FacturxImporter), LogLevel.Debug);
                });


                _logger = factory.CreateLogger<FacturxImporter>();
                _logger.LogInformation("Example log message");
            }
            else
            {
                _logger = logger;
            }
        }

        private XmlDocument LoadXml(PdfStream streamFromPDF)
        {
            if (_xmlDocument == null)
            {
                byte[] bytes;
                var canUnfilter = streamFromPDF.TryUnfilter();
                if (canUnfilter)
                {
                    bytes = streamFromPDF.Value;
                }
                else
                {
                    PdfSharpCore.Pdf.Filters.FlateDecode flate = new PdfSharpCore.Pdf.Filters.FlateDecode();
                    bytes = flate.Decode(streamFromPDF.Value, new PdfSharpCore.Pdf.Filters.FilterParms(null));
                }

                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                string text = uTF8Encoding.GetString(bytes);
                if ((bytes.Length > 3) & (bytes[0] == 239) & (bytes[1] == 187) & (bytes[2] == 191))
                {
                    text = text.Substring(1);
                }
                _xmlDocument = new XmlDocument();
                _xmlDocument.LoadXml(text);
            }
            return _xmlDocument;
        }

        private Models.FacturXMetadata GetFacturXMetadata(PdfDocument document)
        {
            var facturXMetadata = new Models.FacturXMetadata();

            var pdfReference = document.Internals.Catalog.Elements.GetReference("/AF");

            var metadataReference = document.Internals.Catalog.Elements.GetReference("/Metadata");
            if (metadataReference.Value is PdfDictionary)
            {
                var dict = (PdfDictionary)metadataReference.Value;
                var canUnfilter = dict.Stream.TryUnfilter();
                byte[] metadataBytes;
                if (canUnfilter)
                {
                    metadataBytes = dict.Stream.Value;
                }
                else
                {
                    PdfSharpCore.Pdf.Filters.FlateDecode flate = new PdfSharpCore.Pdf.Filters.FlateDecode();
                    metadataBytes = flate.Decode(dict.Stream.Value, new PdfSharpCore.Pdf.Filters.FilterParms(null));
                }

                var xmp = XmpMetaFactory.ParseFromBuffer(metadataBytes);

                var propertyPdfAConformance = xmp.GetProperty(XmpConstants.NsPdfaId, "pdfaid:conformance");
                if (propertyPdfAConformance == null)
                {
                    _logger.LogWarning("No PDF/A conformance.");
                    throw new Exception("No PDF/A conformance.");
                }
                var propertyPdfAPart = xmp.GetProperty(XmpConstants.NsPdfaId, "pdfaid:part");
                if (propertyPdfAPart.Value != "3" && propertyPdfAPart.Value != "4")
                {
                    _logger.LogWarning("PDF/A level is lower than 3.");
                    throw new Exception("PDF/A level is lower than 3.");
                }

                string namespaceUri = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";
                var namespaceURIProperty = xmp.Properties.FirstOrDefault(p => p.Namespace == XmpConstants.NsPdfaSchema && p.Path != null && p.Path.EndsWith("namespaceURI"));
                if (namespaceURIProperty != null)
                {
                    namespaceUri = namespaceURIProperty.Value;
                }

                string facturXPrefix = "fx";
                var facturXPrefixProperty = xmp.Properties.FirstOrDefault(p => p.Namespace == XmpConstants.NsPdfaSchema && p.Path != null && p.Path.EndsWith("prefix"));
                if (facturXPrefixProperty != null)
                {
                    facturXPrefix = facturXPrefixProperty.Value;
                }

                var conformanceLevelProperty = xmp.GetProperty(namespaceUri, $"{facturXPrefix}:ConformanceLevel");
                if (conformanceLevelProperty != null)
                {
                    if (conformanceLevelProperty.Value.ToLowerInvariant() == "minimum")
                    {
                        facturXMetadata.ConformanceLevel = FacturXConformanceLevelType.Minimum;
                    }
                    else if (conformanceLevelProperty.Value.ToLowerInvariant() == "basic")
                    {
                        facturXMetadata.ConformanceLevel = FacturXConformanceLevelType.Basic;
                    }
                    else if (conformanceLevelProperty.Value.ToLowerInvariant() == "basic-wl" || conformanceLevelProperty.Value.ToLowerInvariant() == "basicwl"
                         || conformanceLevelProperty.Value.ToLowerInvariant() == "basic wl")
                    {
                        facturXMetadata.ConformanceLevel = FacturXConformanceLevelType.BasicWL;
                    }
                    else if (conformanceLevelProperty.Value.ToLowerInvariant().Replace(" ", string.Empty) == "en16931")
                    {
                        facturXMetadata.ConformanceLevel = FacturXConformanceLevelType.EN16931;
                    }
                    else
                    {
                        facturXMetadata.ConformanceLevel = FacturXConformanceLevelType.Extended;
                    }
                }

                var documentTypeProperty = xmp.GetProperty(namespaceUri, $"{facturXPrefix}:DocumentType");
                if (documentTypeProperty != null)
                {
                    facturXMetadata.DocumentType = documentTypeProperty.Value;
                }

                var facturXVersionProperty = xmp.GetProperty(namespaceUri, $"{facturXPrefix}:Version");
                if (facturXVersionProperty != null)
                {
                    facturXMetadata.Version = facturXVersionProperty.Value;
                }

                var facturXDocumentFileNameProperty = xmp.GetProperty(namespaceUri, $"{facturXPrefix}:DocumentFileName");
                if (facturXDocumentFileNameProperty != null)
                {
                    facturXMetadata.DocumentFileName = facturXDocumentFileNameProperty.Value;
                }
            }
            return facturXMetadata;
        }

        private PdfStream? GetEmbeddedXmlStream(PdfDocument document, string xmlFileName = "factur-x.xml")
        {
            if (!document.Internals.Catalog.Elements.ContainsKey("/AF"))
            {
                throw new Exception($"Could not find PDF Element AF containing {xmlFileName}");
            }

            var element = document.Internals.Catalog.Elements["/AF"];

            if (element is PdfReference)
            {
                PdfReference objectReferece = element as PdfReference;
                element = objectReferece.Value;
            }


            PdfArray xObject = element as PdfArray;
            if (xObject != null)
            {
                foreach (var pdfElement in xObject.Elements)
                {
                    if (pdfElement is PdfReference)
                    {
                        PdfReference reference = (PdfReference)pdfElement;
                        if (reference.Value is PdfDictionary)
                        {
                            var dict = (PdfDictionary)reference.Value;
                            if (dict.Elements.ContainsKey("/EF") && dict.Elements.ContainsKey("/F"))
                            {
                                var filename = dict.Elements["/F"] as PdfString;
                                if (filename.Value == xmlFileName)
                                {
                                    var efDict = dict.Elements["/EF"] as PdfDictionary;
                                    var fDict = efDict.Elements["/F"] as PdfReference;
                                    var embeded = fDict.Value as PdfDictionary;
                                    return embeded.Stream;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}