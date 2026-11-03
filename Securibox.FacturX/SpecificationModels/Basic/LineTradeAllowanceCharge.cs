namespace Securibox.FacturX.SpecificationModels.Basic
{
    using System.Xml.Serialization;

    /// <summary>
    /// Line allowance/charge (BG-27/BG-28) for the BASIC profile.
    /// BASIC schematron marks CalculationPercent, BasisAmount and CategoryTradeTax as not used here.
    /// </summary>
    public class LineTradeAllowanceCharge
    {
        [XmlElement(
            "ChargeIndicator",
            Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
        )]
        public IndicatorType ChargeIndicator { get; set; }
        public Minimum.Amount ActualAmount { get; set; }
        public string ReasonCode { get; set; }
        public string Reason { get; set; }
    }
}
