using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Basic
{
    public class LineTradeSettlement
    {
        public BasicWL.TradeTax ApplicableTradeTax { get; set; }
        public BasicWL.SpecifiedPeriod BillingSpecifiedPeriod { get; set; }

        [XmlElement]
        public Basic.LineTradeAllowanceCharge[] SpecifiedTradeAllowanceCharge { get; set; }
        public TradeSettlementLineMonetarySummation SpecifiedTradeSettlementLineMonetarySummation { get; set; }
    }
}
