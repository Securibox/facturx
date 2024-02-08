using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class LineTradeSettlement
    {
        public TradeTaxExtended ApplicableTradeTax { get; set; }
        public BasicWL.SpecifiedPeriod BillingSpecifiedPeriod { get; set; }
        [XmlElement]
        public BasicWL.TradeAllowanceCharge[] SpecifiedTradeAllowanceCharge { get; set; }
        public TradeSettlementLineMonetarySummation SpecifiedTradeSettlementLineMonetarySummation { get; set; }
        public EN16931.ReferencedDocumentEN16931 InvoiceReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 AdditionalReferencedDocument { get; set; }
        public TradeAccountingAccount ReceivableSpecifiedTradeAccountingAccount { get; set; }
    }
}