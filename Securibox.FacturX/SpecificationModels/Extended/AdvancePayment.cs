using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class AdvancePayment
    {
        public Minimum.Amount PaidAmount { get; set; }
        public Minimum.IssueDateTime FormattedReceivedDateTime { get; set; }
        [XmlElement]
        public TradeTaxExtended[] IncludedTradeTax { get; set; }
    }
}