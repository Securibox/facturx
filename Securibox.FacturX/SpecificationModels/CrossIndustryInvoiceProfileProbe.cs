namespace Securibox.FacturX.SpecificationModels
{
    using System.Xml.Serialization;

    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100",
        ElementName = "CrossIndustryInvoice"
    )]
    public class CrossIndustryInvoiceProfileProbe
    {
        public Minimum.ExchangedDocumentContext ExchangedDocumentContext { get; set; }
    }
}
