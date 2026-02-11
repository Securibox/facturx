using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class HeaderTradeSettlement
    {
        public Minimum.ID CreditorReferenceID { get; set; }
        public string PaymentReference { get; set; }
        public string TaxCurrencyCode { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public TradeParty PayeeTradeParty { get; set; }
        [XmlElement]
        public TradeSettlementPaymentMeans[] SpecifiedTradeSettlementPaymentMeans { get; set; }
        [XmlElement]
        public TradeTax[] ApplicableTradeTax { get; set; }
        public SpecifiedPeriod BillingSpecifiedPeriod { get; set; }
        [XmlElement]
        public TradeAllowanceCharge[] SpecifiedTradeAllowanceCharge { get; set; }
        public TradePaymentTerms SpecifiedTradePaymentTerms { get; set; }
        public TradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; set; }
        [XmlElement]
        public ReferencedDocument[] InvoiceReferencedDocument { get; set; }
        public TradeAccountingAccount ReceivableSpecifiedTradeAccountingAccount { get; set; }
    }
}
