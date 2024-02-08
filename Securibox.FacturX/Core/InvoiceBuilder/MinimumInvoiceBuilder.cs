using Securibox.FacturX.Models.Enums;
using System.Xml;

namespace Securibox.FacturX.Core
{
    internal class MinimumInvoiceBuilder : InvoiceBuilder
    {
        private Models.Minimum.Invoice invoice;

        internal MinimumInvoiceBuilder(XmlDocument xmlDocument) 
            : base(FacturXConformanceLevelType.Minimum, xmlDocument) 
        { 
            invoice = new Models.Minimum.Invoice();
        }

        protected override void BuildDirectDebit()
        {
            invoice.DirectDebit = GetDirectDebit();
        }

        protected override void BuildHeader()
        {
            invoice.Header = GetHeader();
        }

        protected override void BuildProccessControl()
        {
            invoice.ProccessControl = GetProccessControl();
        }

        protected override void BuildReferences()
        {
            var references = new Models.Minimum.References
            {
                BuyerReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.BuyerReference) as Models.Minimum.BuyerReference,
                PurchaseOrderReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.PurchaseOrderReference) as Models.Minimum.PurchaseOrderReference,
            };

            invoice.References = references; 
        }

        protected override void BuildStakeHolders()
        {
            var stakeHolders = new Models.Minimum.StakeHolders
            {
                Buyer = TradePartyFactory.ConvertActor(TradePartyType.Buyer) as Models.Minimum.Buyer,
                Seller = TradePartyFactory.ConvertActor(TradePartyType.Seller) as Models.Minimum.Seller,
            };

            invoice.StakeHolders = stakeHolders;
        }

        protected override void BuildTotals()
        {
            invoice.Totals = GetTotals(invoice.DirectDebit.InvoiceCurrencyCode);
        }

        internal Models.Minimum.Invoice GetInvoice()
        {
            BuildHeader();
            BuildProccessControl();
            BuildStakeHolders();
            BuildDirectDebit();
            BuildReferences();
            BuildTotals();

            return invoice;
        }
    }
}
