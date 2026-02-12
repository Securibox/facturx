using System.Xml;
using Securibox.FacturX.Core.Line;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Core
{
    internal class EN16931InvoiceBuilder : InvoiceBuilder
    {
        private readonly Models.EN16931.Invoice invoice;

        internal EN16931InvoiceBuilder(XmlDocument xmlDocument)
            : base(FacturXConformanceLevelType.EN16931, xmlDocument)
        {
            invoice = new Models.EN16931.Invoice();
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
            var references = new Models.EN16931.References
            {
                BuyerReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.BuyerReference
                    ) as Models.Minimum.BuyerReference,
                BuyerAccountingReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.BuyerAccountingReference
                    ) as Models.BasicWL.BuyerAccountingReference,
                PurchaseOrderReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.PurchaseOrderReference
                    ) as Models.Minimum.PurchaseOrderReference,
                ContractReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.ContractReference
                    ) as Models.BasicWL.ContractReference,
                TenderOrLotReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.TenderOrLotReference
                    ) as Models.EN16931.TenderOrLotReference,
                ProjectReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.ProjectReference
                    ) as Models.EN16931.ProjectReference,
                SupportingDocumentList = ReferenceFactory
                    .ConvertInvoiceReferenceList(
                        References.ReferenceType.SupportingDocumentReference
                    )
                    ?.Cast<Models.EN16931.SupportingDocument>(),
                InvoicedObjectIdentifier =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.InvoicedObjectIdentifier
                    ) as Models.EN16931.InvoicedObjectIdentifier,
                SalesOrderReference =
                    ReferenceFactory.ConvertInvoiceReference(
                        References.ReferenceType.SalesOrderReference
                    ) as Models.EN16931.SalesOrderReference,
                PreviousInvoiceReferenceList = ReferenceFactory
                    .ConvertInvoiceReferenceList(
                        References.ReferenceType.PreviousInvoiceReferenceList
                    )
                    ?.Cast<Models.BasicWL.PreviousInvoiceReference>(),
            };

            invoice.References = references;
        }

        protected override void BuildStakeHolders()
        {
            var stakeHolders = new Models.EN16931.StakeHolders
            {
                Seller =
                    TradePartyFactory.ConvertActor(TradePartyType.Seller) as Models.EN16931.Seller,
                Buyer =
                    TradePartyFactory.ConvertActor(TradePartyType.Buyer) as Models.EN16931.Buyer,
                SellerTaxRepresentative =
                    TradePartyFactory.ConvertActor(TradePartyType.SellerTaxRepresentative)
                    as Models.BasicWL.SellerTaxRepresentative,
                Payee =
                    TradePartyFactory.ConvertActor(TradePartyType.Payee)
                    as Models.BasicWL.PaymentActor,
            };

            invoice.StakeHolders = stakeHolders;
        }

        protected override void BuildTotals()
        {
            invoice.Totals =
                GetTotals(invoice.DirectDebit.InvoiceCurrencyCode) as Models.EN16931.Totals;
        }

        protected override void BuildPaymentTerms()
        {
            invoice.PaymentTerms = GetPaymentTerms();
        }

        protected override void BuildPaymentInstructions()
        {
            invoice.PaymentInstructions =
                GetPaymentInstructions() as Models.EN16931.PaymentInstructions;
        }

        protected override void BuildDeliveryDetails()
        {
            invoice.DeliveryDetails = GetDeliveryDetails() as Models.EN16931.DeliveryDetails;
        }

        protected override void BuildVatAndAllowanceCharge()
        {
            invoice.TaxDistributionList = GetTaxDistributionList()
                ?.Cast<Models.EN16931.TaxDistribution>();

            if (AllowanceChargeNodeList != null && AllowanceChargeNodeList.Count > 0)
            {
                var allowanceChargeList = new List<Models.BasicWL.AllowanceCharge>();
                foreach (XmlNode allowanceChargeNode in AllowanceChargeNodeList)
                {
                    var allowanceCharge =
                        GetAllowanceCharge(allowanceChargeNode) as Models.BasicWL.AllowanceCharge;
                    allowanceChargeList.Add(allowanceCharge);
                }

                var allowanceList = allowanceChargeList
                    .Where(x => x.ChargeIndicator == false)
                    .ToList();
                invoice.AllowanceList = allowanceList.Any() ? allowanceList : null;

                var chargeList = allowanceChargeList.Where(x => x.ChargeIndicator == true).ToList();
                invoice.ChargeList = chargeList.Any() ? chargeList : null;
            }
        }

        protected override void BuildLines()
        {
            if (LineNodeList == null || LineNodeList.Count == 0)
                return;

            var lineBuilder = new EN16931LineBuilder(TradePartyFactory, ReferenceFactory);

            var lineList = new List<Models.EN16931.InvoiceLine>();
            foreach (XmlNode lineNode in LineNodeList)
            {
                lineBuilder.Reset(lineNode);
                var line = lineBuilder.GetLine();
                lineList.Add(line);
            }

            invoice.LineList = lineList;
        }

        internal Models.EN16931.Invoice GetInvoice()
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
