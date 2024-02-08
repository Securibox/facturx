using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Basic
{
    public class LineTradeSettlement
    {
        public BasicWL.TradeTax ApplicableTradeTax { get; set; }
        public BasicWL.SpecifiedPeriod BillingSpecifiedPeriod { get; set; }
        [XmlElement]
        public BasicWL.TradeAllowanceCharge[] SpecifiedTradeAllowanceCharge { get; set; }
        public TradeSettlementLineMonetarySummation SpecifiedTradeSettlementLineMonetarySummation { get; set; }
    }
}