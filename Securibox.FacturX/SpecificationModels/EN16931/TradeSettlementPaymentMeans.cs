using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class TradeSettlementPaymentMeans
    {
        public string TypeCode { get; set; }
        public string Information { get; set; }
        public TradeSettlementFinancialCard ApplicableTradeSettlementFinancialCard { get; set; }
        public BasicWL.DebtorFinancialAccount PayerPartyDebtorFinancialAccount { get; set; }
        [XmlElement]
        public CreditorFinancialAccount[] PayeePartyCreditorFinancialAccount { get; set; }
        public CreditorFinancialInstitution PayeeSpecifiedCreditorFinancialInstitution { get; set; }
    }
}