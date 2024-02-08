using Microsoft.Extensions.Logging;
using Securibox.FacturX.Models.Enums;
using System.Xml;

namespace Securibox.FacturX.Core
{
    internal class Xml2ModelConverter
    {
        private XmlDocument _xmlDocument;
        private FacturXConformanceLevelType _conformanceLevelType;
        private ILogger<Xml2ModelConverter> _logger;
        private readonly IList<ErrorReport> _errorReports;

       

        private int _version;


        internal Xml2ModelConverter(XmlDocument xmlDocument, FacturXConformanceLevelType conformanceLevelType, ILogger<Xml2ModelConverter> logger = null)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }

            if (logger == null)
            {
                using ILoggerFactory factory = LoggerFactory.Create(delegate (ILoggingBuilder builder)
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning).AddFilter("System", LogLevel.Warning).AddFilter(nameof(Xml2ModelConverter), LogLevel.Debug);
                });
                logger = factory.CreateLogger<Xml2ModelConverter>();
            }
            _logger = logger;

            _xmlDocument = xmlDocument;
            _conformanceLevelType = conformanceLevelType;

            if (_xmlDocument.OuterXml.Contains("<rsm:CrossIndustryDocument"))
            {
                _version = 1;
            }
            if (_xmlDocument.OuterXml.Contains("<rsm:CrossIndustryInvoice"))
            {
                _version = 2;
            }

            _errorReports = new List<ErrorReport>();

            
        }

        internal Models.Minimum.Invoice Convert()
        {
            if (_conformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                var minimumBuilder = new MinimumInvoiceBuilder(_xmlDocument);
                return minimumBuilder.GetInvoice();
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.BasicWL)
            {
                var basicWLBuilder = new BasicWLInvoiceBuilder(_xmlDocument);
                return basicWLBuilder.GetInvoice();
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Basic)
            {
                var basicBuilder = new BasicInvoiceBuilder(_xmlDocument);
                return basicBuilder.GetInvoice();
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                var en16931Builder = new EN16931InvoiceBuilder(_xmlDocument);
                return en16931Builder.GetInvoice();
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var extendedBuilder = new ExtendedInvoiceBuilder(_xmlDocument);
                return extendedBuilder.GetInvoice();
            }

            return null;
        }
    }
}


