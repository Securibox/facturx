using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class TradeSettlementPaymentMeans
    {
        public string TypeCode { get; set; }
        public DebtorFinancialAccount PayerPartyDebtorFinancialAccount { get; set; }
        
        public CreditorFinancialAccount PayeePartyCreditorFinancialAccount { get; set; }
    }
}