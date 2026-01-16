using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class TradeAllowanceCharge : Basic.LineTradeAllowanceCharge
    {
        public decimal CalculationPercent { get; set; }
    }
}