using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Basic
{
    public class TradeAllowanceCharge
    {
        [XmlElement(
            "ChargeIndicator",
            Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
        )]
        public IndicatorType ChargeIndicator { get; set; }
        public Minimum.Amount ActualAmount { get; set; }
    }
}
