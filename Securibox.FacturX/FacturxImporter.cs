using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.Filters;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.IO.enums;
using Securibox.FacturX.Models;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Schematron.Helpers;
using Securibox.FacturX.SpecificationModels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using XmpCore;
using static PdfSharpCore.Pdf.PdfDictionary;

namespace Securibox.FacturX
{
    public class FacturxImporter : IDisposable
    {
        private ILogger<FacturxImporter> _logger;
        private MemoryStream _pdfFileStream;
        private XmlDocument _xmlDocument;
        private FacturXMetadata _facturXMetadata;
        private PdfDocument _pdfDocument;
        public List<ValidationReport> validationReport;

        public FacturxImporter(Stream pdfStream, ILogger<FacturxImporter>? logger = null)
        {
            InitializeLogger(logger);
            if (pdfStream is MemoryStream memoryStream)
            {
                _pdfFileStream = memoryStream;
            }
            else
            {
                using (var temp = pdfStream)
                {
                    var ms = new MemoryStream();
                    temp.CopyTo(ms);
                    ms.Position = 0;
                    _pdfFileStream = ms;
                }            
            }

            _pdfDocument = PdfReader.Open(_pdfFileStream, accuracy: PdfReadAccuracy.Moderate);
        }

        public FacturxImporter(string pdfFilename, ILogger<FacturxImporter>? logger = null)
        {
            InitializeLogger(logger);
            if (!File.Exists(pdfFilename))
            {
                throw new FileNotFoundException("File not found", pdfFilename);
            }

            using (var fileStream = File.OpenRead(pdfFilename))
            {
                var memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                _pdfFileStream = memoryStream;
                _pdfDocument = PdfReader.Open(_pdfFileStream, accuracy: PdfReadAccuracy.Moderate);
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
            validationReport = new List<ValidationReport>();
            var facturXMetadata = GetMetadata();
            var xmlPdfStream = GetEmbeddedXmlStream(_pdfDocument, facturXMetadata.DocumentFileName);
            if (xmlPdfStream == null || xmlPdfStream.Length == 0)
            {
                _logger!.LogCritical("Could not read embedded XML file.");
                throw new IOException("Could not read embedded XML file.");
            }

            LoadXml(xmlPdfStream);
            FacturxXsdValidator.ValidateXml(_xmlDocument, facturXMetadata.ConformanceLevel);
            var schValidationResult = FacturxSchematronValidator.ValidateXml(_xmlDocument, facturXMetadata.ConformanceLevel);
            if (!schValidationResult._isSuccessfullValidation)
            {
                var errors = schValidationResult._results.Where(x => x.IsError == true || x.IsWarning == true).ToList();
                for (int i = 0; i < errors.Count; i++)
                {
                    validationReport.Add(errors[i]);
                }

                return false;
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
            var metadataReference = document.Internals.Catalog.Elements.GetReference("/Metadata");
            if (metadataReference.Value is PdfDictionary)
            {
                var dict = (PdfDictionary)metadataReference.Value;
                byte[] metadataBytes = new byte[0];
                if (dict?.Stream == null)
                {
                    ArgumentNullException.ThrowIfNull(_pdfFileStream);

                    _pdfFileStream.Position = 0;
                    var pdfText = Encoding.UTF8.GetString(_pdfFileStream.ToArray());
                    int pos = pdfText.IndexOf("<x:xmpmeta", StringComparison.Ordinal);
                    if (pos >= 0)
                    {
                        int endPos = pdfText.IndexOf("</x:xmpmeta>", pos, StringComparison.Ordinal);
                        if (endPos >= 0)
                        {
                            endPos += "</x:xmpmeta>".Length;
                            string xmpXml = pdfText.Substring(pos, endPos - pos);
                            metadataBytes = Encoding.UTF8.GetBytes(xmpXml);
                        }
                    }            
                }
                else
                {
                    var canUnfilter = dict.Stream.TryUnfilter();
                    if (canUnfilter)
                    {
                        metadataBytes = dict.Stream.Value;
                    }
                    else
                    {
                        PdfSharpCore.Pdf.Filters.FlateDecode flate = new PdfSharpCore.Pdf.Filters.FlateDecode();
                        metadataBytes = flate.Decode(dict.Stream.Value, new PdfSharpCore.Pdf.Filters.FilterParms(null));
                    }
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
            if (document.Internals.Catalog.Elements.ContainsKey("/AF"))
            {
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
                                        if (efDict != null)
                                        {
                                            var fDict = efDict.Elements["/F"] as PdfReference;
                                            if (fDict?.Value is PdfDictionary fileSpec && fileSpec.Stream != null)
                                            {
                                                return fileSpec.Stream;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (document.Internals.Catalog.Elements.ContainsKey("/Names"))
            {
                var names = document.Internals.Catalog.Elements.GetDictionary("/Names");
                if (names != null && names.Elements.ContainsKey("/EmbeddedFiles"))
                {
                    var efTree = names.Elements.GetDictionary("/EmbeddedFiles");
                    var kids = efTree.Elements.GetArray("/Names");

                    for (int i = 0; i < kids.Elements.Count; i += 2)
                    {
                        var name = kids.Elements[i] as PdfString;
                        var fileSpec = kids.Elements[i + 1] as PdfReference;

                        if (fileSpec?.Value is PdfDictionary dict)
                        {
                            var fileName = dict.Elements.GetString("/F");
                            if (fileName == xmlFileName)
                            {
                                var efDict = dict.Elements.GetDictionary("/EF");
                                if (efDict != null)
                                {
                                    var fDict = efDict.Elements["/F"] as PdfReference;
                                    if (fDict?.Value is PdfDictionary embeddedDict)
                                    {
                                        return embeddedDict.Stream;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var obj in document.Internals.GetAllObjects())
            {
                if (obj is PdfDictionary dict && dict.Elements.ContainsKey("/Type") &&
                    dict.Elements["/Type"] is PdfName type && type.Value == "/Filespec")
                {
                    var filename = dict.Elements["/F"] as PdfString;
                    if (filename != null && filename.Value == xmlFileName)
                    {
                        var efDict = dict.Elements["/EF"] as PdfDictionary;
                        var fDict = efDict?.Elements["/F"] as PdfReference;
                        if (fDict?.Value is PdfDictionary fileSpec && fileSpec.Stream != null)
                        {
                            return fileSpec.Stream;
                        }
                    }
                }
            }

            foreach (var page in document.Pages)
            {
                var annots = page.Elements["/Annots"] as PdfArray;
                if (annots == null)
                {
                    continue;
                }

                foreach (var annot in annots.Elements.OfType<PdfReference>())
                {
                    if (annot.Value is PdfDictionary annotDict &&
                        annotDict.Elements["/Subtype"] is PdfName subtype &&
                        subtype.Value == "/FileAttachment")
                    {
                        var fsRef = annotDict.Elements["/FS"] as PdfReference;
                        if (fsRef?.Value is PdfDictionary fsDict)
                        {
                            var filename = fsDict.Elements["/F"] as PdfString;
                            if (filename != null && filename.Value == xmlFileName)
                            {
                                var efDict = fsDict.Elements["/EF"] as PdfDictionary;
                                var fDict = efDict?.Elements["/F"] as PdfReference;
                                if (fDict?.Value is PdfDictionary fileSpec && fileSpec.Stream != null)
                                {
                                    return fileSpec.Stream;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void Dispose()
        {
            _pdfFileStream?.Dispose();
            _pdfDocument?.Close();
        }
    }
}