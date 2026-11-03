namespace Securibox.FacturX.SpecificationModels.EN16931
{
    using System.Xml.Serialization;

    /// <summary>
    /// Line allowance/charge (BG-27/BG-28) for the EN16931 and EXTENDED profiles.
    /// Their schematrons mark CategoryTradeTax (and SequenceNumeric/BasisQuantity in EXTENDED) as not used here.
    /// </summary>
    public class LineTradeAllowanceCharge
    {
        [XmlElement(
            "ChargeIndicator",
            Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
        )]
        public Basic.IndicatorType ChargeIndicator { get; set; }
        public decimal? CalculationPercent { get; set; }

        [XmlIgnore]
        public bool CalculationPercentSpecified => CalculationPercent.HasValue;
        public Minimum.Amount BasisAmount { get; set; }
        public Minimum.Amount ActualAmount { get; set; }
        public string ReasonCode { get; set; }
        public string Reason { get; set; }
    }
}
