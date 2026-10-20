namespace Securibox.FacturX.SpecificationModels.Extended
{
    using System.Xml.Serialization;

    public class TradeAllowanceChargeExtended
    {
        public Basic.IndicatorType ChargeIndicator { get; set; }
        public string SequenceNumeric { get; set; }
        public decimal? CalculationPercent { get; set; }

        [XmlIgnore]
        public bool CalculationPercentSpecified => CalculationPercent.HasValue;
        public Minimum.Amount BasisAmount { get; set; }
        public Basic.Quantity BasisQuantity { get; set; }
        public Minimum.Amount ActualAmount { get; set; }
        public string ReasonCode { get; set; }
        public string Reason { get; set; }
        public TradeTaxExtended CategoryTradeTax { get; set; }
    }
}
