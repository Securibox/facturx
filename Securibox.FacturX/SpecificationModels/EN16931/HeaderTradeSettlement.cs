using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class HeaderTradeSettlement
    {
        public string CreditorReferenceID { get; set; }
        public string PaymentReference { get; set; }
        public string TaxCurrencyCode { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public TradePartyEN16931 PayeeTradeParty { get; set; }

        [XmlElement]
        public TradeSettlementPaymentMeans[] SpecifiedTradeSettlementPaymentMeans { get; set; }

        [XmlElement]
        public TradeTaxEN16931[] ApplicableTradeTax { get; set; }
        public BasicWL.SpecifiedPeriod BillingSpecifiedPeriod { get; set; }

        [XmlElement]
        public BasicWL.TradeAllowanceCharge[] SpecifiedTradeAllowanceCharge { get; set; }
        public BasicWL.TradePaymentTerms SpecifiedTradePaymentTerms { get; set; }
        public TradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; set; }

        [XmlElement]
        public ReferencedDocumentEN16931[] InvoiceReferencedDocument { get; set; }
        public TradeAccountingAccountEN16931 ReceivableSpecifiedTradeAccountingAccount { get; set; }
    }
}
