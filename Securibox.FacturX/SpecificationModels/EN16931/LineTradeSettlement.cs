using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class LineTradeSettlement
    {
        public TradeTaxEN16931 ApplicableTradeTax { get; set; }
        public BasicWL.SpecifiedPeriod BillingSpecifiedPeriod { get; set; }

        [XmlElement]
        public BasicWL.TradeAllowanceCharge[] SpecifiedTradeAllowanceCharge { get; set; }
        public Basic.TradeSettlementLineMonetarySummation SpecifiedTradeSettlementLineMonetarySummation { get; set; }
        public ReferencedDocumentEN16931 AdditionalReferencedDocument { get; set; }
        public TradeAccountingAccountEN16931 ReceivableSpecifiedTradeAccountingAccount { get; set; }
    }
}
