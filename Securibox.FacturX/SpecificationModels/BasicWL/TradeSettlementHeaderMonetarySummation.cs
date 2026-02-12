using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class TradeSettlementHeaderMonetarySummation
    {
        public Minimum.Amount LineTotalAmount { get; set; }
        public Minimum.Amount ChargeTotalAmount { get; set; }
        public Minimum.Amount AllowanceTotalAmount { get; set; }
        public Minimum.Amount TaxBasisTotalAmount { get; set; }

        [XmlElement]
        public Minimum.Amount[] TaxTotalAmount { get; set; }
        public Minimum.Amount GrandTotalAmount { get; set; }
        public Minimum.Amount TotalPrepaidAmount { get; set; }
        public Minimum.Amount DuePayableAmount { get; set; }
    }
}
