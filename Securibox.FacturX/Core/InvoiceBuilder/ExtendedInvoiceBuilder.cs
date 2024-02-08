using Securibox.FacturX.Core.Line;
using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Extended;
using Securibox.FacturX.Models.Extended.Payment;
using System.Xml;

namespace Securibox.FacturX.Core
{
    internal class ExtendedInvoiceBuilder : InvoiceBuilder
    {
        private Models.Extended.Invoice invoice;

        private readonly XmlNodeList? SpecifiedLogisticsServiceChargeNodeList;
        private readonly XmlNode? TaxCurrencyExchangeNode;
        private readonly XmlNodeList? AdvancePaymentNodeList;

        internal ExtendedInvoiceBuilder(XmlDocument xmlDocument) : base(FacturXConformanceLevelType.Extended, xmlDocument) 
        {
            SpecifiedLogisticsServiceChargeNodeList = TradeSettlementNode.SelectNodes("*[local-name() = 'SpecifiedLogisticsServiceCharge']");
            TaxCurrencyExchangeNode = TradeSettlementNode.SelectSingleNode("*[local-name() = 'TaxApplicableTradeCurrencyExchange']");
            AdvancePaymentNodeList = TradeSettlementNode.SelectNodes("*[local-name() = 'SpecifiedAdvancePayment']");

            invoice = new Models.Extended.Invoice();
        }

        protected override void BuildDirectDebit()
        {
            invoice.DirectDebit = GetDirectDebit() as Models.Extended.DirectDebit;
        }

        protected override void BuildHeader()
        {
            invoice.Header = GetHeader() as Models.Extended.Header;
        }

        protected override void BuildProccessControl()
        {
            invoice.ProccessControl = GetProccessControl() as Models.Extended.ProccessControl;

        }

        protected override void BuildReferences()
        {
            var references = new Models.Extended.References
            {
                BuyerReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.BuyerReference) as Models.Minimum.BuyerReference,
                PurchaseOrderReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.PurchaseOrderReference) as Models.Extended.PurchaseOrderReference,
                ContractReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.ContractReference) as Models.Extended.ContractReference,
                TenderOrLotReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.TenderOrLotReference) as Models.Extended.TenderOrLotReference,
                ProjectReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.ProjectReference) as Models.EN16931.ProjectReference,
                SupportingDocumentList = ReferenceFactory.ConvertInvoiceReferenceList(References.ReferenceType.SupportingDocumentReference)?.Cast<Models.Extended.SupportingDocument>(),
                InvoicedObjectIdentifier = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.InvoicedObjectIdentifier) as Models.Extended.InvoicedObjectIdentifier,
                SalesOrderReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.SalesOrderReference) as Models.Extended.SalesOrderReference,
                QuotationReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.QuotationReference) as Models.Extended.QuotationReference,
                UltimateCustomerOrderReferenceList = ReferenceFactory.ConvertInvoiceReferenceList(References.ReferenceType.UltimateCustomerOrderReferenceList)?.Cast<Models.Extended.UltimateCustomerOrderReference>(),
                BuyerAccountingReference = ReferenceFactory.ConvertInvoiceReference(References.ReferenceType.BuyerAccountingReference) as Models.Extended.BuyerAccountingReference,
                PreviousInvoiceReferenceList = ReferenceFactory.ConvertInvoiceReferenceList(References.ReferenceType.PreviousInvoiceReferenceList)?.Cast<Models.Extended.PreviousInvoiceReference>(),
            };

            invoice.References = references;
        }

        protected override void BuildStakeHolders()
        {
            var stakeHolders = new Models.Extended.StakeHolders
            {
                Seller = TradePartyFactory.ConvertActor(TradePartyType.Seller) as Models.Extended.Seller,
                Buyer = TradePartyFactory.ConvertActor(TradePartyType.Buyer) as Models.Extended.Buyer,
                SellerTaxRepresentative = TradePartyFactory.ConvertActor(TradePartyType.SellerTaxRepresentative) as Models.Extended.SellerTaxRepresentative,
                BuyerTaxRepresentative = TradePartyFactory.ConvertActor(TradePartyType.BuyerTaxRepresentative) as Models.Extended.BuyerTaxRepresentative,
                SalesAgent = TradePartyFactory.ConvertActor(TradePartyType.SalesAgent) as Models.Extended.SalesAgent,
                BuyerAgent = TradePartyFactory.ConvertActor(TradePartyType.BuyerAgent) as Models.Extended.BuyerAgent,
                ProductEndUser = TradePartyFactory.ConvertActor(TradePartyType.ProductEndUser) as Models.Extended.ProductEndUser,
                Invoicee = TradePartyFactory.ConvertActor(TradePartyType.SalesAgent) as Models.Extended.Invoicee,
                Invoicer = TradePartyFactory.ConvertActor(TradePartyType.Invoicer) as Models.Extended.Invoicer,
                Payee = TradePartyFactory.ConvertActor(TradePartyType.Payee) as Models.Extended.PaymentActor,
                Payer = TradePartyFactory.ConvertActor(TradePartyType.Payer) as Models.Extended.PaymentActor,
                PayeeMultiplePayment = TradePartyFactory.ConvertActor(TradePartyType.PayeeMultiplePaymentsAndPayee) as Models.Extended.PaymentActor,
            };

            invoice.StakeHolders = stakeHolders;
        }

        protected override void BuildTotals()
        {
            invoice.Totals = GetTotals(invoice.DirectDebit.InvoiceCurrencyCode) as Models.Extended.Totals;
        }

        protected override void BuildPaymentTerms() 
        {
            invoice.PaymentTerms = GetPaymentTerms() as Models.Extended.PaymentTerms;
        }

        protected override void BuildPaymentInstructions() 
        {
            invoice.PaymentInstructions = GetPaymentInstructions() as Models.EN16931.PaymentInstructions;
        }

        protected override void BuildDeliveryDetails() 
        {
            invoice.DeliveryDetails = GetDeliveryDetails() as Models.Extended.DeliveryDetails;
        }

        protected override void BuildVatAndAllowanceCharge() 
        {
            invoice.TaxDistributionList = GetTaxDistributionList()?.Cast<Models.Extended.TaxDistribution>();


            if (AllowanceChargeNodeList != null && AllowanceChargeNodeList.Count > 0)
            {
                var allowanceChargeList = new List<Models.Extended.AllowanceCharge>();
                foreach (XmlNode allowanceChargeNode in AllowanceChargeNodeList)
                {
                    var allowanceCharge = GetAllowanceCharge(allowanceChargeNode) as Models.Extended.AllowanceCharge;
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

            var lineBuilder = new ExtendedLineBuilder(TradePartyFactory, ReferenceFactory);

            var lineList = new List<Models.Extended.InvoiceLine>();
            foreach (XmlNode lineNode in LineNodeList)
            {
                lineBuilder.Reset(lineNode);
                var line = lineBuilder.GetLine();
                lineList.Add(line);
            }

            invoice.LineList = lineList;
        }

        protected void BuildTaxCurrencyExchange() 
        {
            if (TaxCurrencyExchangeNode == null)
                return;

            var conversionRateNode = TaxCurrencyExchangeNode.SelectSingleNode("*[local-name() = 'ConversionRate']");
            var conversionRateDateNode = TaxCurrencyExchangeNode.SelectSingleNode("*[local-name() = 'ConversionRateDateTime']");

            invoice.TaxCurrencyExchange = new TaxCurrencyExchange
            {
                SourceCurrencyCode = TaxCurrencyExchangeNode.SelectSingleNode("*[local-name() = 'SourceCurrencyCode']")?.InnerText,
                TargetCurrencyCode = TaxCurrencyExchangeNode.SelectSingleNode("*[local-name() = 'TargetCurrencyCode']")?.InnerText,
                ConversionRate = XmlParsingHelpers.ExtractDecimal(conversionRateNode),
                ConversionRateDate = XmlParsingHelpers.ExtractDateTime(conversionRateDateNode),
            };
        }

        protected void BuildAdvancePaymentList() 
        {
            if (AdvancePaymentNodeList == null || AdvancePaymentNodeList.Count == 0)
                return;

            var advancePaymentList = new List<AdvancePayment>();
            foreach (XmlNode advancePaymentNode in AdvancePaymentNodeList)
            {
                var advancePayment = GetAdvancePayment(advancePaymentNode);
                advancePaymentList.Add(advancePayment);
            }

            invoice.AdvancePaymentList = advancePaymentList;
        }

        protected void BuildLogisticsServiceChargeList() 
        {
            if (SpecifiedLogisticsServiceChargeNodeList == null || SpecifiedLogisticsServiceChargeNodeList.Count == 0)
                return;

            invoice.LogisticsServiceChargeList = new List<LogisticsServiceCharge>();
            foreach (XmlNode specifiedLogisticsNode in SpecifiedLogisticsServiceChargeNodeList)
            {
                var logisticsServiceCharge = GetLogisticsServiceCharge(specifiedLogisticsNode);
                invoice.LogisticsServiceChargeList.Add(logisticsServiceCharge);
            }
        }

        #region LogisticsServiceCharge
        private Models.Extended.AppliedTax GetAppliedTax(XmlNode logisticsServiceChargeNode)
        {
            var vatRateNode = logisticsServiceChargeNode.SelectSingleNode("*[local-name() = 'RateApplicablePercent']");

            return new Models.Extended.AppliedTax
            {
                VatType = logisticsServiceChargeNode.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText,
                VatRate = XmlParsingHelpers.ExtractDecimal(vatRateNode),
                VatCategoryCode = logisticsServiceChargeNode.SelectSingleNode("*[local-name() = 'CategoryCode']")?.InnerText,
            };
        }

        private IEnumerable<AppliedTax>? GetLogisticsServiceChargeAppliedTaxList(XmlNode logisticsServiceChargeNode)
        {
            var appliedTaxNodeList = logisticsServiceChargeNode.SelectNodes("*[local-name() = 'AppliedTradeTax']");
            if (appliedTaxNodeList == null || appliedTaxNodeList.Count == 0)
                return null;

            var appliedTaxList = new List<AppliedTax>();
            foreach (XmlNode taxNode in appliedTaxNodeList)
            {
                appliedTaxList.Add(GetAppliedTax(taxNode));
            }

            return appliedTaxList;
        }

        internal Models.Extended.LogisticsServiceCharge GetLogisticsServiceCharge(XmlNode logisticsServiceChargeNode)
        {
            var appliedAmountNode = logisticsServiceChargeNode.SelectSingleNode("*[local-name() = 'AppliedAmount']");

            return new Models.Extended.LogisticsServiceCharge
            {
                Description = logisticsServiceChargeNode.SelectSingleNode("*[local-name() = 'Description']")?.InnerText,
                AppliedAmount = XmlParsingHelpers.ExtractDecimal(appliedAmountNode),
                AppliedTaxList = GetLogisticsServiceChargeAppliedTaxList(logisticsServiceChargeNode)
            };
        }
        #endregion

        #region AdvancePayment

        private IncludedTax GetAdvancePaymentIncludedTax(XmlNode includedTaxNode)
        {
            var vatAmount = includedTaxNode.SelectSingleNode("*[local-name() = 'CalculatedAmount']");
            var vatRate = includedTaxNode.SelectSingleNode("*[local-name() = 'RateApplicablePercent']");

            var includedTax = new IncludedTax
            {
                VatAmount = XmlParsingHelpers.ExtractDecimal(vatAmount),
                VatType = includedTaxNode.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText,
                VatCategoryCode = includedTaxNode.SelectSingleNode("*[local-name() = 'CategoryCode']")?.InnerText,
                VatRate = XmlParsingHelpers.ExtractDecimal(vatRate),
            };

            includedTax.VatExemptionReason = new Reason
            {
                Text = includedTaxNode.SelectSingleNode("*[local-name() = 'ExemptionReason']")?.InnerText,
                Code = includedTaxNode.SelectSingleNode("*[local-name() = 'ExemptionReasonCode']")?.InnerText
            };

            return includedTax;
        }

        private IEnumerable<IncludedTax>? GetAdvancePaymentIncludedTaxList(XmlNode advancePaymentNode)
        {
            var includedTaxNodeList = advancePaymentNode.SelectNodes("*[local-name() = 'IncludedTradeTax']");
            if (includedTaxNodeList == null || includedTaxNodeList.Count == 0)
                return null;

            var includedTaxList = new List<IncludedTax>();
            foreach (XmlNode includedTaxNode in includedTaxNodeList)
            {
                var includedTax = GetAdvancePaymentIncludedTax(includedTaxNode);
                includedTaxList.Add(includedTax);
            }

            return includedTaxList;
        }

        private AdvancePayment GetAdvancePayment(XmlNode advancePaymentNode)
        {
            var paidAmountNode = advancePaymentNode.SelectSingleNode("*[local-name() = 'PaidAmount']");
            var receivedPaymentDateNode = advancePaymentNode.SelectSingleNode("*[local-name() = 'FormattedReceivedDateTime']");

            return new AdvancePayment
            {
                PaidAmount = XmlParsingHelpers.ExtractDecimal(paidAmountNode),
                ReceivedPaymentDate = XmlParsingHelpers.ExtractDateTime(receivedPaymentDateNode),
                IncludedTaxList = GetAdvancePaymentIncludedTaxList(advancePaymentNode)
            };
        }
        #endregion

        internal Models.Extended.Invoice GetInvoice()
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
            BuildAdvancePaymentList();
            BuildLogisticsServiceChargeList();
            BuildTaxCurrencyExchange();

            return invoice;
        }
    }
}
