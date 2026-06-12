using System.Xml;
using System.Xml.XPath;

namespace Securibox.FacturX.Core
{
    /// <summary>
    /// Centralized, hardened XML loading helpers used for all UNTRUSTED input
    /// (e.g. foreign Factur-X / ZUGFeRD invoice XML extracted from third-party PDFs).
    ///
    /// All readers produced here disable DTD processing and external entity/resolver
    /// resolution, which prevents XML External Entity (XXE) attacks such as local
    /// file disclosure, SSRF and entity-expansion (billion laughs) denial of service.
    /// </summary>
    public static class SecureXml
    {
        /// <summary>
        /// Reader settings that are safe for untrusted XML: no DTD, no external resolver,
        /// and a bounded character count to mitigate entity-expansion style DoS.
        /// </summary>
        public static XmlReaderSettings CreateSafeReaderSettings()
        {
            return new XmlReaderSettings
            {
                // Reject any DOCTYPE declaration outright. This is the strongest XXE
                // defense and also makes entity-expansion ("billion laughs") attacks
                // impossible, since no entities can be declared without a DTD.
                DtdProcessing = DtdProcessing.Prohibit,
                // No resolver => external entities, schemas and includes are never fetched.
                XmlResolver = null,
                CloseInput = false,
            };
        }

        /// <summary>
        /// Creates a hardened <see cref="XmlReader"/> over the given stream.
        /// </summary>
        public static XmlReader CreateReader(Stream stream)
        {
            return XmlReader.Create(stream, CreateSafeReaderSettings());
        }

        /// <summary>
        /// Loads untrusted XML from a stream into an <see cref="XmlDocument"/> with
        /// DTD/external-entity processing disabled.
        /// </summary>
        public static XmlDocument LoadDocument(Stream stream)
        {
            var doc = new XmlDocument { XmlResolver = null };
            using (var reader = CreateReader(stream))
            {
                doc.Load(reader);
            }
            return doc;
        }

        /// <summary>
        /// Loads untrusted XML from a string into an <see cref="XmlDocument"/> with
        /// DTD/external-entity processing disabled.
        /// </summary>
        public static XmlDocument LoadDocument(string xml)
        {
            var doc = new XmlDocument { XmlResolver = null };
            using (var stringReader = new StringReader(xml))
            using (var reader = XmlReader.Create(stringReader, CreateSafeReaderSettings()))
            {
                doc.Load(reader);
            }
            return doc;
        }

        /// <summary>
        /// Loads untrusted XML from a stream into an <see cref="XPathDocument"/> with
        /// DTD/external-entity processing disabled.
        /// </summary>
        public static XPathDocument LoadXPathDocument(Stream stream)
        {
            using (var reader = CreateReader(stream))
            {
                return new XPathDocument(reader);
            }
        }
    }
}
