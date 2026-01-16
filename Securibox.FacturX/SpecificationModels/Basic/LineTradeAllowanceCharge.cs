namespace Securibox.FacturX.SpecificationModels.Basic
{
    using Securibox.FacturX.SpecificationModels.BasicWL;
    using System.Xml.Serialization;

    public class LineTradeAllowanceCharge
    {
        [XmlElement("ChargeIndicator", Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100")]
        public IndicatorType ChargeIndicator { get; set; }
        public Minimum.Amount BasisAmount { get; set; }
        public Minimum.Amount ActualAmount { get; set; }
        public string ReasonCode { get; set; }
        public string Reason { get; set; }
        public TradeTax CategoryTradeTax { get; set; }
    }
}