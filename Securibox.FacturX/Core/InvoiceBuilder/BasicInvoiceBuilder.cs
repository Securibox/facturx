using Securibox.FacturX.Core.Line;
using Securibox.FacturX.Models.Enums;
using System.Xml;

namespace Securibox.FacturX.Core
{
    internal class BasicInvoiceBuilder : InvoiceBuilder
    {
        private Models.Basic.Invoice invoice;

        internal BasicInvoiceBuilder(XmlDocument xmlDocument) : base(FacturXConformanceLevelType.Basic, xmlDocument) 
        { 
            invoice = new Models.Basic.Invoice();
        }

        protected override void BuildDirectDebit()
        {
            invoice.DirectDebit = GetDirectDebit() as Models.BasicWL.DirectDebit;
        }

        protected override void BuildHeader()
        {
            invoice.Header = GetHeader() as Models.BasicWL.Header;
        }

        protected override void BuildProccessControl()
        {
            invoice.ProccessControl = GetProccessControl();

        }

        protected override void BuildReferences()
        {
            var references = new Models.BasicWL.References
            {
                BuyerReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.BuyerReference) as Models.Minimum.BuyerReference,
                PurchaseOrderReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.PurchaseOrderReference) as Models.Minimum.PurchaseOrderReference,
                ContractReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.ContractReference) as Models.BasicWL.ContractReference,
                BuyerAccountingReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.BuyerAccountingReference) as Models.BasicWL.BuyerAccountingReference,
                PreviousInvoiceReferenceList = ReferenceFactory.ConvertInvoiceReferenceList(References.ReferenceType.PreviousInvoiceReferenceList)?.Cast<Models.BasicWL.PreviousInvoiceReference>(),
            };

            invoice.References = references;
        }

        protected override void BuildStakeHolders()
        {
            var stakeHolders = new Models.BasicWL.StakeHolders
            {
                Seller = TradePartyFactory.ConvertActor(TradePartyType.Seller) as Models.BasicWL.Seller,
                Buyer = TradePartyFactory.ConvertActor(TradePartyType.Buyer) as Models.BasicWL.Buyer,
                SellerTaxRepresentative = TradePartyFactory.ConvertActor(TradePartyType.SellerTaxRepresentative) as Models.BasicWL.SellerTaxRepresentative,
                Payee = TradePartyFactory.ConvertActor(TradePartyType.Payee) as Models.BasicWL.PaymentActor,
            };

            invoice.StakeHolders = stakeHolders;
        }

        protected override void BuildTotals()
        {
            invoice.Totals = GetTotals(invoice.DirectDebit.InvoiceCurrencyCode) as Models.BasicWL.Totals;
        }

        protected override void BuildPaymentTerms() 
        {
            invoice.PaymentTerms = GetPaymentTerms();

        }

        protected override void BuildPaymentInstructions() 
        {
            invoice.PaymentInstructions = GetPaymentInstructions();

        }

        protected override void BuildDeliveryDetails() 
        {
            invoice.DeliveryDetails = GetDeliveryDetails();
        }

        protected override void BuildVatAndAllowanceCharge() 
        {
            invoice.TaxDistributionList = GetTaxDistributionList();

            if (AllowanceChargeNodeList != null && AllowanceChargeNodeList.Count > 0)
            {
                var allowanceChargeList = new List<Models.BasicWL.AllowanceCharge>();
                foreach (XmlNode allowanceChargeNode in AllowanceChargeNodeList)
                {
                    var allowanceCharge = GetAllowanceCharge(allowanceChargeNode) as Models.BasicWL.AllowanceCharge;
                    allowanceChargeList.Add(allowanceCharge);
                }

                var allowanceList = allowanceChargeList.Where(x => x.ChargeIndicator == false).ToList();
                invoice.AllowanceList = allowanceList.Any() ? allowanceList : null;

                var chargeList = allowanceChargeList.Where(x => x.ChargeIndicator == true).ToList();
                invoice.ChargeList = chargeList.Any() ? chargeList : null;
            }
        }

        protected override void BuildLines()
        {
            if (LineNodeList == null || LineNodeList.Count == 0)
                return;

            var lineBuilder = new BasicLineBuilder(TradePartyFactory, ReferenceFactory);
            
            var lineList = new List<Models.Basic.InvoiceLine>();
            foreach (XmlNode lineNode in LineNodeList)
            {
                lineBuilder.Reset(lineNode);
                var line = lineBuilder.GetLine();
                lineList.Add(line);
            }

            invoice.LineList = lineList;
        }

        internal Models.BasicWL.Invoice GetInvoice()
        {
            BuildHeader();
            BuildProccessControl();
            BuildStakeHolders();
            BuildDirectDebit();
            BuildReferences();
            BuildTotals();
            BuildPaymentInstructions();
            BuildPaymentTerms();
            BuildDeliveryDetails();
            BuildVatAndAllowanceCharge();
            BuildLines();

            return invoice;
        }
    }
}
