using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class HeaderTradeSettlement
    {
        public Minimum.ID CreditorReferenceID { get; set; }
        public string PaymentReference { get; set; }
        public string TaxCurrencyCode { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string InvoiceIssuerReference { get; set; }
        public TradeParty InvoicerTradeParty { get; set; }
        public TradeParty InvoiceeTradeParty { get; set; }
        public TradeParty PayeeTradeParty { get; set; }
        public TradeParty PayerTradeParty { get; set; }
        public TradeCurrencyExchange TaxApplicableTradeCurrencyExchange { get; set; }
        [XmlElement]
        public EN16931.TradeSettlementPaymentMeans[] SpecifiedTradeSettlementPaymentMeans { get; set; }
        [XmlElement]
        public TradeTaxExtended[] ApplicableTradeTax { get; set; }
        public BasicWL.SpecifiedPeriod BillingSpecifiedPeriod { get; set; }
        [XmlElement]
        public TradeAllowanceChargeExtended[] SpecifiedTradeAllowanceCharge { get; set; }
        [XmlElement]
        public LogisticsServiceCharge[] SpecifiedLogisticsServiceCharge { get; set; }
        public TradePaymentTerms SpecifiedTradePaymentTerms { get; set; }
        public EN16931.TradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; set; }
        [XmlElement]
        public EN16931.ReferencedDocumentEN16931[] InvoiceReferencedDocument { get; set; }
        public TradeAccountingAccount ReceivableSpecifiedTradeAccountingAccount { get; set; }
        [XmlElement]
        public AdvancePayment[] SpecifiedAdvancePayment { get; set; }
    }
}