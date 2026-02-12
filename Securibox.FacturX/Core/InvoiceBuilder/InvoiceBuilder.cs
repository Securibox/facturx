using System.Xml;
using Securibox.FacturX.Models.Basic;
using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Extended;
using Securibox.FacturX.Models.Extended.Payment;
using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Core
{
    internal abstract class InvoiceBuilder
    {
        private readonly FacturXConformanceLevelType _conformanceLevelType;

        protected readonly ReferenceFactory ReferenceFactory;
        protected readonly TradePartyFactory TradePartyFactory;

        private readonly XmlNode ExchangedDocumentNode;
        private readonly XmlNodeList? ExchangedDocumentNoteList;
        private readonly XmlNode ExchangedDocumentContextNode;

        private readonly XmlNode TradeAgreementNode;
        private readonly XmlNode? TradeDeliveryNode;
        private readonly XmlNode? TradePaymentTermsNode;
        private readonly XmlNode? TradePaymentMeansNode;
        private readonly XmlNode MonetarySummationNode;

        protected readonly XmlNode TradeSettlementNode;
        protected readonly XmlNodeList? AllowanceChargeNodeList;
        protected readonly XmlNodeList? TaxDistributionNodeList;

        protected readonly XmlNodeList? LineNodeList;

        protected InvoiceBuilder(
            FacturXConformanceLevelType conformanceLevelType,
            XmlDocument xmlDocument
        )
        {
            _conformanceLevelType = conformanceLevelType;

            TradePartyFactory = new TradePartyFactory(conformanceLevelType, xmlDocument);
            ReferenceFactory = new ReferenceFactory(conformanceLevelType, xmlDocument);

            ExchangedDocumentNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ExchangedDocument']"
            )!;
            ExchangedDocumentNoteList = ExchangedDocumentNode?.SelectNodes(
                "*[local-name() = 'IncludedNote']"
            );

            ExchangedDocumentContextNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ExchangedDocumentContext']"
            )!;

            TradeAgreementNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ApplicableHeaderTradeAgreement']"
            )!;
            TradeSettlementNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ApplicableHeaderTradeSettlement']"
            )!;
            TradeDeliveryNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ApplicableHeaderTradeDelivery']"
            );

            TradePaymentTermsNode = TradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'SpecifiedTradePaymentTerms']"
            );
            TradePaymentMeansNode = TradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'SpecifiedTradeSettlementPaymentMeans']"
            );

            AllowanceChargeNodeList = TradeSettlementNode.SelectNodes(
                "*[local-name() = 'SpecifiedTradeAllowanceCharge']"
            );
            TaxDistributionNodeList = TradeSettlementNode.SelectNodes(
                "*[local-name() = 'ApplicableTradeTax']"
            );

            MonetarySummationNode = TradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'SpecifiedTradeSettlementHeaderMonetarySummation']"
            )!;

            LineNodeList = xmlDocument.SelectNodes(
                "//*[local-name() = 'IncludedSupplyChainTradeLineItem']"
            );
        }

        protected abstract void BuildHeader();
        protected abstract void BuildProccessControl();
        protected abstract void BuildStakeHolders();
        protected abstract void BuildDirectDebit();
        protected abstract void BuildReferences();
        protected abstract void BuildTotals();

        protected virtual void BuildPaymentTerms() { }

        protected virtual void BuildPaymentInstructions() { }

        protected virtual void BuildDeliveryDetails() { }

        protected virtual void BuildVatAndAllowanceCharge() { }

        protected virtual void BuildLines() { }

        #region ProccessControl
        protected Models.Minimum.ProccessControl GetProccessControl()
        {
            var businessProccessType = ExchangedDocumentContextNode
                .SelectSingleNode(
                    "*[local-name() = 'BusinessProcessSpecifiedDocumentContextParameter']"
                )
                ?.SelectSingleNode("*[local-name() = 'ID']")
                ?.InnerText;

            var specificationIdentification = ExchangedDocumentContextNode
                .SelectSingleNode("*[local-name() = 'GuidelineSpecifiedDocumentContextParameter']")!
                .SelectSingleNode("*[local-name() = 'ID']")!
                .InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var proccessControl = new Models.Extended.ProccessControl
                {
                    BusinessProcessType = businessProccessType,
                    SpecificationIdentification = specificationIdentification,
                };

                var testIndicatorNode = ExchangedDocumentContextNode.SelectSingleNode(
                    "*[local-name() = 'TestIndicator']"
                );

                if (testIndicatorNode != null)
                {
                    var testIndicator = testIndicatorNode
                        .SelectSingleNode("*[local-name() = 'Indicator']")!
                        .InnerText;

                    bool.TryParse(testIndicator, out bool indicator);

                    proccessControl.TestIndicator = indicator;
                }

                return proccessControl;
            }

            return new Models.Minimum.ProccessControl
            {
                BusinessProcessType = businessProccessType,
                SpecificationIdentification = specificationIdentification,
            };
        }
        #endregion

        #region Header
        private Models.BasicWL.Note GetNote(XmlNode noteNode)
        {
            var content = noteNode.SelectSingleNode("*[local-name() = 'Content']")!.InnerText;
            var subjectCode = noteNode
                .SelectSingleNode("*[local-name() = 'SubjectCode']")
                ?.InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var contentCode = noteNode
                    .SelectSingleNode("*[local-name() = 'ContentCode']")
                    ?.InnerText;
                return new Models.Extended.Note(content, subjectCode, contentCode);
            }
            else
            {
                return new Models.BasicWL.Note(content, subjectCode);
            }
        }

        private IEnumerable<Models.BasicWL.Note>? GetNoteList()
        {
            if (ExchangedDocumentNoteList == null || ExchangedDocumentNoteList.Count == 0)
                return null;

            var noteList = new List<Models.BasicWL.Note>();
            foreach (XmlNode noteNode in ExchangedDocumentNoteList)
                noteList.Add(GetNote(noteNode));

            return noteList;
        }

        protected Models.Minimum.Header GetHeader()
        {
            var invoiceNumber = ExchangedDocumentNode
                .SelectSingleNode("*[local-name() = 'ID']")!
                .InnerText;
            var invoiceType = ExchangedDocumentNode
                .SelectSingleNode("*[local-name() = 'TypeCode']")!
                .InnerText;

            var emissionDateNode = ExchangedDocumentNode
                .SelectSingleNode("*[local-name() = 'IssueDateTime']")!
                .SelectSingleNode("*[local-name() = 'DateTimeString']")!;

            var emissionDate = XmlParsingHelpers.ExtractDateTime(emissionDateNode);

            if (_conformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                return new Models.Minimum.Header(invoiceNumber, invoiceType, emissionDate);
            }
            else if (
                _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                || _conformanceLevelType == FacturXConformanceLevelType.Basic
                || _conformanceLevelType == FacturXConformanceLevelType.EN16931
            )
            {
                var header = new Models.BasicWL.Header(invoiceNumber, invoiceType, emissionDate);
                header.AddBasicWLNoteList(GetNoteList());

                return header;
            }
            else
            {
                var header = new Models.Extended.Header(invoiceNumber, invoiceType, emissionDate);
                header.AddExtendedNoteList(GetNoteList()?.Cast<Models.Extended.Note>());

                var invoiceName = ExchangedDocumentNode
                    .SelectSingleNode("*[local-name() = 'Name']")
                    ?.InnerText;
                header.AddInvoiceName(invoiceName);

                var copyIndicatorNode = ExchangedDocumentNode.SelectSingleNode(
                    "*[local-name() = 'CopyIndicator']"
                );
                if (copyIndicatorNode != null)
                {
                    var copyIndicatorString = copyIndicatorNode
                        .SelectSingleNode("*[local-name() = 'Indicator']")!
                        .InnerText;
                    bool.TryParse(copyIndicatorString, out bool copyIndicator);

                    header.AddCopyIndicator(copyIndicator);
                }

                var languageNodeList = ExchangedDocumentNode.SelectNodes(
                    "*[local-name() = 'LanguageID']"
                );
                if (languageNodeList != null && languageNodeList.Count > 0)
                {
                    var languageList = languageNodeList.Cast<XmlNode>().Select(x => x.InnerText);
                    header.AddLanguageList(languageList);
                }

                var effectiveSpecifiedPeriodNode = ExchangedDocumentNode.SelectSingleNode(
                    "*[local-name() = 'EffectiveSpecifiedPeriod']"
                );
                if (effectiveSpecifiedPeriodNode != null)
                {
                    var completeDateTime = effectiveSpecifiedPeriodNode
                        .SelectSingleNode("*[local-name() = 'CompleteDateTime']")!
                        .SelectSingleNode("*[local-name() = 'DateTimeString']")!;

                    var effectiveSpecifiedPeriod = XmlParsingHelpers.ExtractDateTime(
                        completeDateTime
                    );

                    header.AddEffectiveSpecifiedPeriod(effectiveSpecifiedPeriod);
                }

                return header;
            }
        }
        #endregion

        #region DirectDebit
        internal Models.Minimum.DirectDebit GetDirectDebit()
        {
            var currencyCode = TradeSettlementNode
                .SelectSingleNode("*[local-name() = 'InvoiceCurrencyCode']")!
                .InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                return new Models.Minimum.DirectDebit(currencyCode);
            }
            else
            {
                var creditorReferenceId = TradeSettlementNode
                    .SelectSingleNode("*[local-name() = 'CreditorReferenceID']")
                    ?.InnerText;
                var paymentReference = TradeSettlementNode
                    .SelectSingleNode("*[local-name() = 'PaymentReference']")
                    ?.InnerText;
                var taxCurrencyCode = TradeSettlementNode
                    .SelectSingleNode("*[local-name() = 'TaxCurrencyCode']")
                    ?.InnerText;

                if (
                    _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                    || _conformanceLevelType == FacturXConformanceLevelType.Basic
                    || _conformanceLevelType == FacturXConformanceLevelType.EN16931
                )
                {
                    return new Models.BasicWL.DirectDebit(
                        currencyCode,
                        creditorReferenceId,
                        paymentReference,
                        taxCurrencyCode
                    );
                }
                else
                {
                    var invoiceIssuerReference = TradeSettlementNode
                        .SelectSingleNode("*[local-name() = 'InvoiceIssuerReference']")
                        ?.InnerText;
                    return new Models.Extended.DirectDebit(
                        currencyCode,
                        creditorReferenceId,
                        paymentReference,
                        taxCurrencyCode,
                        invoiceIssuerReference
                    );
                }
            }
        }
        #endregion

        #region Delivery
        private IEnumerable<ShippingTransportation>? GetShippingTransportationList()
        {
            var shippingTransportationNodeList = TradeDeliveryNode
                ?.SelectSingleNode("*[local-name() = 'RelatedSupplyChainConsignment']")
                ?.SelectNodes("*[local-name() = 'SpecifiedLogisticsTransportMovement']");

            if (shippingTransportationNodeList == null)
                return null;

            var shippingTransportationList = new List<ShippingTransportation>();
            foreach (XmlNode shippingTransportationNode in shippingTransportationNodeList)
            {
                var modeCode = shippingTransportationNode
                    .SelectSingleNode("*[local-name() = 'ModeCode']")
                    ?.InnerText;
                var shippingTransportation = new ShippingTransportation { ModeCode = modeCode };
                shippingTransportationList.Add(shippingTransportation);
            }

            return shippingTransportationList;
        }

        private Models.BasicWL.DeliveryPeriod? GetInvoicingPeriod()
        {
            var deliveryPeriodNode = TradeSettlementNode?.SelectSingleNode(
                "*[local-name() = 'BillingSpecifiedPeriod']"
            );
            if (deliveryPeriodNode == null)
                return null;

            var startDate = default(DateTime?);
            var startDateNode = deliveryPeriodNode.SelectSingleNode(
                "*[local-name() = 'StartDateTime']"
            );
            if (startDateNode != null)
            {
                var dateStringNode = startDateNode.SelectSingleNode(
                    "*[local-name() = 'DateTimeString']"
                )!;
                startDate = XmlParsingHelpers.ExtractDateTime(dateStringNode);
            }

            var endDate = default(DateTime?);
            var endDateNode = deliveryPeriodNode.SelectSingleNode(
                "*[local-name() = 'EndDateTime']"
            );
            if (endDateNode != null)
            {
                var dateStringNode = endDateNode.SelectSingleNode(
                    "*[local-name() = 'DateTimeString']"
                )!;
                endDate = XmlParsingHelpers.ExtractDateTime(dateStringNode);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var description = deliveryPeriodNode
                    .SelectSingleNode("*[local-name() = 'Description']")
                    ?.InnerText;
                return new Models.Extended.DeliveryPeriod(startDate, endDate, description);
            }

            return new Models.BasicWL.DeliveryPeriod(startDate, endDate);
        }

        protected Models.BasicWL.DeliveryDetails? GetDeliveryDetails()
        {
            if (TradeDeliveryNode == null)
                return null;

            var actualDeliveryDate = default(DateTime?);

            var actualDeliveryEventNode = TradeDeliveryNode.SelectSingleNode(
                "*[local-name() = 'ActualDeliverySupplyChainEvent']"
            );

            if (actualDeliveryEventNode != null)
            {
                var occurrenceDateTimeNode = actualDeliveryEventNode.SelectSingleNode(
                    "*[local-name() = 'OccurrenceDateTime']"
                );
                if (occurrenceDateTimeNode != null)
                {
                    var actualDeliveryDateNode = occurrenceDateTimeNode.SelectSingleNode(
                        "*[local-name() = 'DateTimeString']"
                    )!;
                    actualDeliveryDate = XmlParsingHelpers.ExtractDateTime(actualDeliveryDateNode);
                }
            }

            var deliveryType = TradeAgreementNode
                .SelectSingleNode("*[local-name() = 'ApplicableTradeDeliveryTerms']")
                ?.SelectSingleNode("*[local-name() = 'DeliveryTypeCode']")
                ?.InnerText;

            if (
                _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                || _conformanceLevelType == FacturXConformanceLevelType.Basic
            )
            {
                return new Models.BasicWL.DeliveryDetails
                {
                    DespatchAdviceReference =
                        ReferenceFactory.ConvertInvoiceReference(
                            References.ReferenceType.DespatchAdviceReference
                        ) as Models.BasicWL.DespacthAdviceReference,
                    DeliverToAddress =
                        TradePartyFactory.ConvertActor(TradePartyType.DeliverToAddress)
                        as Models.BasicWL.DeliverToAddress,
                    DeliveryDate = actualDeliveryDate,
                    InvoicingPeriod = GetInvoicingPeriod(),
                };
            }
            else if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.DeliveryDetails
                {
                    DespatchAdviceReference =
                        ReferenceFactory.ConvertInvoiceReference(
                            References.ReferenceType.DespatchAdviceReference
                        ) as Models.BasicWL.DespacthAdviceReference,
                    DeliverToAddress =
                        TradePartyFactory.ConvertActor(TradePartyType.DeliverToAddress)
                        as Models.BasicWL.DeliverToAddress,
                    DeliveryDate = actualDeliveryDate,
                    InvoicingPeriod = GetInvoicingPeriod(),
                    ReceivingAdviceReference =
                        ReferenceFactory.ConvertInvoiceReference(
                            References.ReferenceType.ReceivingAdviceReference
                        ) as Models.EN16931.ReceivingAdviceReference,
                };
            }
            else if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                return new Models.Extended.DeliveryDetails
                {
                    DeliverToAddress =
                        TradePartyFactory.ConvertActor(TradePartyType.DeliverToAddress)
                        as Models.Extended.DeliverToAddress,
                    DeliverFromAddress =
                        TradePartyFactory.ConvertActor(TradePartyType.DeliverFromAddress)
                        as Models.Extended.DeliverFromAddress,
                    UltimateDeliverToAddress =
                        TradePartyFactory.ConvertActor(TradePartyType.UltimateDeliverToAddress)
                        as Models.Extended.UltimateDeliverToAddress,
                    DeliveryDate = actualDeliveryDate,
                    InvoicingPeriod = GetInvoicingPeriod() as Models.Extended.DeliveryPeriod,
                    DespatchAdviceReference =
                        ReferenceFactory.ConvertInvoiceReference(
                            References.ReferenceType.DespatchAdviceReference
                        ) as Models.Extended.DespacthAdviceReference,
                    ReceivingAdviceReference =
                        ReferenceFactory.ConvertInvoiceReference(
                            References.ReferenceType.ReceivingAdviceReference
                        ) as Models.Extended.ReceivingAdviceReference,
                    DeliveryNoteReference =
                        ReferenceFactory.ConvertInvoiceReference(
                            References.ReferenceType.DeliveryNoteReference
                        ) as Models.Extended.DeliveryNoteReference,
                    DeliveryType = deliveryType,
                    ShippingTransportList = GetShippingTransportationList(),
                };
            }

            return null;
        }
        #endregion

        #region PaymentTerms
        private PaymentPenaltyDiscount? GetPaymentPenaltyDiscount(
            XmlNode paymentPenaltyDiscountNode,
            bool isDiscount
        )
        {
            var basisDateTimeNode = paymentPenaltyDiscountNode.SelectSingleNode(
                "*[local-name() = 'BasisDateTime']"
            );
            var basisDateTime = XmlParsingHelpers.ExtractDateTime(basisDateTimeNode);

            var paymentDiscountBaseAmountNode = paymentPenaltyDiscountNode.SelectSingleNode(
                "*[local-name() = 'BasisAmount']"
            );
            var paymentDiscountBaseAmount = XmlParsingHelpers.ExtractDecimal(
                paymentDiscountBaseAmountNode
            );

            var paymentDiscountCalculationPercentNode = paymentPenaltyDiscountNode.SelectSingleNode(
                "*[local-name() = 'CalculationPercent']"
            );
            var paymentDiscountCalculationPercent = XmlParsingHelpers.ExtractDecimal(
                paymentDiscountCalculationPercentNode
            );

            var actualDiscountAmountNode = isDiscount
                ? paymentPenaltyDiscountNode.SelectSingleNode(
                    "*[local-name() = 'ActualDiscountAmount']"
                )
                : paymentPenaltyDiscountNode.SelectSingleNode(
                    "*[local-name() = 'ActualPenaltyAmount']"
                );

            var actualDiscountAmount = XmlParsingHelpers.ExtractDecimal(actualDiscountAmountNode);

            var paymentPenaltyDiscount = new PaymentPenaltyDiscount
            {
                BaseDateTime = basisDateTime,
                Percentage = paymentDiscountCalculationPercent,
                BaseAmount = paymentDiscountBaseAmount,
                ActualAmount = actualDiscountAmount,
            };

            var basisPeriodMeasureNode = paymentPenaltyDiscountNode.SelectSingleNode(
                "*[local-name() = 'BasisPeriodMeasure']"
            );
            if (basisPeriodMeasureNode != null)
            {
                paymentPenaltyDiscount.BasePeriodMeasure = new PeriodMeasure
                {
                    Measure = XmlParsingHelpers.ExtractDecimal(basisPeriodMeasureNode),
                    UnitCode = XmlParsingHelpers.ExtractAttribute(
                        basisPeriodMeasureNode,
                        "unitCode"
                    ),
                };
            }

            return paymentPenaltyDiscount;
        }

        private PaymentPenaltyDiscount? GetPaymentPenalty()
        {
            var paymentPenalty = TradePaymentTermsNode?.SelectSingleNode(
                "*[local-name() = 'ApplicableTradePaymentPenaltyTerms']"
            );
            if (paymentPenalty == null)
                return null;

            return GetPaymentPenaltyDiscount(paymentPenalty, false);
        }

        private PaymentPenaltyDiscount? GetPaymentDiscount()
        {
            var paymentDiscount = TradePaymentTermsNode?.SelectSingleNode(
                "*[local-name() = 'ApplicableTradePaymentDiscountTerms']"
            );
            if (paymentDiscount == null)
                return null;

            return GetPaymentPenaltyDiscount(paymentDiscount, true);
        }

        protected Models.BasicWL.PaymentTerms GetPaymentTerms()
        {
            var conditions = TradePaymentTermsNode
                ?.SelectSingleNode("*[local-name() = 'Description']")
                ?.InnerText;

            var dueDate = default(DateTime?);
            var dueDateNode = TradePaymentTermsNode
                ?.SelectSingleNode("*[local-name() = 'DueDateDateTime']")
                ?.SelectSingleNode("*[local-name() = 'DateTimeString']");

            if (dueDateNode != null)
            {
                dueDate = XmlParsingHelpers.ExtractDateTime(dueDateNode);
            }

            var mandateReference = TradePaymentTermsNode
                ?.SelectSingleNode("*[local-name() = 'DirectDebitMandateID']")
                ?.InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var partialPaymentAmount = default(decimal?);
                var partialPaymentAmountNode = TradePaymentTermsNode?.SelectSingleNode(
                    "*[local-name() = 'PartialPaymentAmount']"
                );
                if (partialPaymentAmountNode != null)
                {
                    partialPaymentAmount = XmlParsingHelpers.ExtractDecimal(
                        partialPaymentAmountNode
                    );
                }

                return new Models.Extended.PaymentTerms
                {
                    Conditions = conditions,
                    DueDate = dueDate,
                    MandateReference = mandateReference,
                    PartialPaymentAmount = partialPaymentAmount,
                    PaymentPenalty = GetPaymentPenalty(),
                    PaymentDiscount = GetPaymentDiscount(),
                };
            }

            return new Models.BasicWL.PaymentTerms
            {
                Conditions = conditions,
                DueDate = dueDate,
                MandateReference = mandateReference,
            };
        }
        #endregion

        #region PaymentInstructions
        private CreditTransfer GetCreditTransfer(XmlNode creditTransferNode)
        {
            var ibanId = creditTransferNode
                .SelectSingleNode("*[local-name() = 'IBANID']")!
                .InnerText;
            var proprietaryId = creditTransferNode
                .SelectSingleNode("*[local-name() = 'ProprietaryID']")
                ?.InnerText;

            if (
                _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                || _conformanceLevelType == FacturXConformanceLevelType.Basic
            )
            {
                return new CreditTransfer(ibanId, proprietaryId);
            }

            var paymentAccountName = creditTransferNode
                .SelectSingleNode("*[local-name() = 'AccountName']")
                ?.InnerText;
            return new Models.EN16931.CreditTransfer(ibanId, proprietaryId, paymentAccountName);
        }

        private IEnumerable<CreditTransfer>? GetCreditTransferList()
        {
            var creditTransferNodeList = TradePaymentMeansNode?.SelectNodes(
                "*[local-name() = 'PayeePartyCreditorFinancialAccount']"
            );
            if (creditTransferNodeList == null || creditTransferNodeList.Count == 0)
                return null;

            var creditTransferList = new List<CreditTransfer>();
            foreach (XmlNode creditTransferNode in creditTransferNodeList)
                creditTransferList.Add(GetCreditTransfer(creditTransferNode));

            return creditTransferList;
        }

        protected Models.BasicWL.PaymentInstructions? GetPaymentInstructions()
        {
            if (TradePaymentMeansNode == null)
                return null;

            var debitedAccountIban = TradePaymentMeansNode
                .SelectSingleNode("*[local-name() = 'PayerPartyDebtorFinancialAccount']")
                ?.SelectSingleNode("*[local-name() = 'IBANID']")
                ?.InnerText;

            var paymentTypeCode = TradePaymentMeansNode
                .SelectSingleNode("*[local-name() = 'TypeCode']")!
                .InnerText;

            var creditTransferList = GetCreditTransferList();

            if (
                _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                || _conformanceLevelType == FacturXConformanceLevelType.Basic
            )
            {
                var basicWLPaymentInstructions = new Models.BasicWL.PaymentInstructions(
                    paymentTypeCode,
                    debitedAccountIban
                );
                basicWLPaymentInstructions.AddBasicWLCreditTransferList(creditTransferList);
                return basicWLPaymentInstructions;
            }

            var paymentMeansInformation = TradePaymentMeansNode
                .SelectSingleNode("*[local-name() = 'Information']")
                ?.InnerText;

            var paymentServiceProvider = TradePaymentMeansNode
                .SelectSingleNode("*[local-name() = 'PayeeSpecifiedCreditorFinancialInstitution']")
                ?.SelectSingleNode("*[local-name() = 'BICID']")
                ?.InnerText;

            var paymentInstructions = new Models.EN16931.PaymentInstructions(
                paymentTypeCode,
                debitedAccountIban
            );
            paymentInstructions.AddEN16931CreditTransferList(
                creditTransferList?.Cast<Models.EN16931.CreditTransfer>()
            );
            paymentInstructions.AddPaymentInformation(paymentMeansInformation);
            paymentInstructions.AddPaymentServiceProvider(paymentServiceProvider);

            var paymentCardNode = TradePaymentMeansNode?.SelectSingleNode(
                "*[local-name() = 'ApplicableTradeSettlementFinancialCard']"
            );
            if (paymentCardNode != null)
            {
                var accountNumber = paymentCardNode
                    .SelectSingleNode("*[local-name() = 'ID']")!
                    .InnerText;
                var cardHolderName = paymentCardNode
                    .SelectSingleNode("*[local-name() = 'CardholderName']")
                    ?.InnerText;

                var paymentCardInformation = new Models.EN16931.CardInformation(
                    accountNumber,
                    cardHolderName
                );
                paymentInstructions.AddPaymentCardInformation(paymentCardInformation);
            }

            return paymentInstructions;
        }
        #endregion

        #region TaxDistribution
        private Models.BasicWL.TaxDistribution GetTaxDistribution(XmlNode taxDistributionNode)
        {
            var categoryAmount = default(decimal?);
            var vatAmountNode = taxDistributionNode.SelectSingleNode(
                "*[local-name() = 'CalculatedAmount']"
            );
            if (vatAmountNode != null)
            {
                categoryAmount = XmlParsingHelpers.ExtractDecimal(vatAmountNode);
            }

            var categoryBaseAmount = default(decimal?);
            var baseAmountNode = taxDistributionNode.SelectSingleNode(
                "*[local-name() = 'BasisAmount']"
            );
            if (baseAmountNode != null)
            {
                categoryBaseAmount = XmlParsingHelpers.ExtractDecimal(baseAmountNode);
            }

            var categoryRate = default(decimal?);
            var vatRateNode = taxDistributionNode.SelectSingleNode(
                "*[local-name() = 'RateApplicablePercent']"
            );
            if (vatRateNode != null)
            {
                categoryRate = XmlParsingHelpers.ExtractDecimal(vatRateNode);
            }

            var categoryCode = taxDistributionNode
                .SelectSingleNode("*[local-name() = 'CategoryCode']")
                ?.InnerText;
            var categoryType = taxDistributionNode
                .SelectSingleNode("*[local-name() = 'TypeCode']")
                ?.InnerText;

            var addedTaxPointDateCode = taxDistributionNode
                .SelectSingleNode("*[local-name() = 'DueDateTypeCode']")
                ?.InnerText;

            var exemptionReason = new Reason
            {
                Text = taxDistributionNode
                    .SelectSingleNode("*[local-name() = 'ExemptionReason']")
                    ?.InnerText,
                Code = taxDistributionNode
                    .SelectSingleNode("*[local-name() = 'ExemptionReasonCode']")
                    ?.InnerText,
            };

            if (
                _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                || _conformanceLevelType == FacturXConformanceLevelType.Basic
            )
            {
                return new Models.BasicWL.TaxDistribution
                {
                    CategoryAmount = categoryAmount,
                    CategoryBaseAmount = categoryBaseAmount,
                    CategoryCode = categoryCode,
                    CategoryRate = categoryRate,
                    CategoryType = categoryType,
                    AddedTaxPointDateCode = addedTaxPointDateCode,
                    ExemptionReason = exemptionReason,
                };
            }
            else
            {
                var addedTaxPointDate = default(DateTime?);
                var vatTaxPointDateNode = taxDistributionNode.SelectSingleNode(
                    "*[local-name() = 'TaxPointDate']"
                );
                if (vatTaxPointDateNode != null)
                {
                    addedTaxPointDate = XmlParsingHelpers.ExtractDateTime(vatTaxPointDateNode);
                }

                var allowanceChargeBaseAmount = default(decimal?);
                var allowanceChargeBaseAmountNode = taxDistributionNode.SelectSingleNode(
                    "*[local-name() = 'AllowanceChargeBasisAmount']"
                );
                if (allowanceChargeBaseAmountNode != null)
                {
                    allowanceChargeBaseAmount = XmlParsingHelpers.ExtractDecimal(
                        allowanceChargeBaseAmountNode
                    );
                }

                var lineTotalBaseAmount = default(decimal?);
                var lineTotalBaseAmountNode = taxDistributionNode.SelectSingleNode(
                    "*[local-name() = 'LineTotalBasisAmount']"
                );
                if (lineTotalBaseAmountNode != null)
                {
                    lineTotalBaseAmount = XmlParsingHelpers.ExtractDecimal(lineTotalBaseAmountNode);
                }

                if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
                {
                    return new Models.EN16931.TaxDistribution
                    {
                        CategoryAmount = categoryAmount,
                        CategoryBaseAmount = categoryBaseAmount,
                        CategoryCode = categoryCode,
                        CategoryRate = categoryRate,
                        CategoryType = categoryType,
                        AddedTaxPointDateCode = addedTaxPointDateCode,
                        ExemptionReason = exemptionReason,
                        AddedTaxPointDate = addedTaxPointDate,
                    };
                }
                else
                {
                    return new Models.Extended.TaxDistribution
                    {
                        CategoryAmount = categoryAmount,
                        CategoryBaseAmount = categoryBaseAmount,
                        CategoryCode = categoryCode,
                        CategoryRate = categoryRate,
                        CategoryType = categoryType,
                        AddedTaxPointDateCode = addedTaxPointDateCode,
                        ExemptionReason = exemptionReason,
                        AddedTaxPointDate = addedTaxPointDate,
                        AllowanceChargeBaseAmount = allowanceChargeBaseAmount,
                        LineTotalBaseAmount = lineTotalBaseAmount,
                    };
                }
            }
        }

        protected IEnumerable<Models.BasicWL.TaxDistribution>? GetTaxDistributionList()
        {
            if (TaxDistributionNodeList == null || TaxDistributionNodeList.Count == 0)
                return null;

            var taxDistributionList = new List<Models.BasicWL.TaxDistribution>();
            foreach (XmlNode taxDistributionNode in TaxDistributionNodeList)
            {
                var taxDistribution = GetTaxDistribution(taxDistributionNode);
                taxDistributionList.Add(taxDistribution);
            }

            return taxDistributionList;
        }
        #endregion

        #region Totals
        private TotalAmount? GetTotalAmountWithoutVat()
        {
            var amount = default(decimal?);
            var taxBasisTotalAmountNode = MonetarySummationNode.SelectSingleNode(
                "*[local-name() = 'TaxBasisTotalAmount']"
            );
            if (taxBasisTotalAmountNode != null)
            {
                amount = XmlParsingHelpers.ExtractDecimal(taxBasisTotalAmountNode);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var currency = XmlParsingHelpers.ExtractAttribute(
                    taxBasisTotalAmountNode,
                    "currencyID"
                );
                return new TotalAmountAndCurrency { Amount = amount, Currency = currency };
            }

            return new TotalAmount { Amount = amount };
        }

        private TotalAmount? GetTotalAmountWithVat()
        {
            var amount = default(decimal?);
            var grandTotalAmountNode = MonetarySummationNode.SelectSingleNode(
                "*[local-name() = 'GrandTotalAmount']"
            );
            if (grandTotalAmountNode != null)
            {
                amount = XmlParsingHelpers.ExtractDecimal(grandTotalAmountNode);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var currency = XmlParsingHelpers.ExtractAttribute(
                    grandTotalAmountNode,
                    "currencyID"
                );
                return new TotalAmountAndCurrency { Amount = amount, Currency = currency };
            }
            return new TotalAmount { Amount = amount };
        }

        private TotalAmountAndCurrency? GetTotalVatAmountInAccountingCurrency(
            string? invoiceCurrency
        )
        {
            var totalVatAmountNodes = MonetarySummationNode
                ?.SelectNodes("*[local-name() = 'TaxTotalAmount']")
                ?.Cast<XmlNode>();
            if (
                totalVatAmountNodes == null
                || totalVatAmountNodes.Any(x =>
                    XmlParsingHelpers.ExtractAttribute(x, "currencyID") == null
                )
            )
                return null;

            var totalVatAmountInAccountingCurrency = totalVatAmountNodes.FirstOrDefault(x =>
                !XmlParsingHelpers.ExtractAttribute(x, "currencyID")!.Equals(invoiceCurrency)
            );
            if (totalVatAmountInAccountingCurrency == null)
                return null;

            var amount = XmlParsingHelpers.ExtractDecimal(totalVatAmountInAccountingCurrency);
            if (amount == null)
                return null;

            var currency = XmlParsingHelpers.ExtractAttribute(
                totalVatAmountInAccountingCurrency,
                "currencyID"
            );

            return new TotalAmountAndCurrency { Amount = amount, Currency = currency };
        }

        protected Models.Minimum.Totals GetTotals(string invoiceCurrency)
        {
            var amount = default(decimal?);
            var currency = default(string);
            var totalVatAmountNode = MonetarySummationNode.SelectSingleNode(
                "*[local-name() = 'TaxTotalAmount']"
            );
            if (totalVatAmountNode != null)
            {
                amount = XmlParsingHelpers.ExtractDecimal(totalVatAmountNode);
                currency = totalVatAmountNode.Attributes!["currencyID"]!.Value;
            }

            var totalVatAmount = new TotalAmountAndCurrency
            {
                Amount = amount,
                Currency = currency,
            };

            var amountToBePaid = default(decimal);
            var amountToBePaidNode = MonetarySummationNode.SelectSingleNode(
                "*[local-name() = 'DuePayableAmount']"
            );
            if (amountToBePaidNode != null)
            {
                amountToBePaid = XmlParsingHelpers.ExtractDecimal(amountToBePaidNode);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                return new Models.Minimum.Totals
                {
                    TotalVatAmount = totalVatAmount,
                    TotalAmountWithVat = GetTotalAmountWithVat(),
                    TotalAmountWithoutVat = GetTotalAmountWithoutVat(),
                    AmountToBePaid = amountToBePaid,
                };
            }
            else
            {
                var netAmountSum = default(decimal?);
                var lineTotalAmountNode = MonetarySummationNode.SelectSingleNode(
                    "*[local-name() = 'LineTotalAmount']"
                );
                if (lineTotalAmountNode != null)
                {
                    netAmountSum = XmlParsingHelpers.ExtractDecimal(lineTotalAmountNode);
                }

                var allowanceSum = default(decimal?);
                var totalAllowanceNode = MonetarySummationNode.SelectSingleNode(
                    "*[local-name() = 'AllowanceTotalAmount']"
                );
                if (totalAllowanceNode != null)
                {
                    allowanceSum = XmlParsingHelpers.ExtractDecimal(totalAllowanceNode);
                }

                var chargeSum = default(decimal?);
                var totalChargesNode = MonetarySummationNode.SelectSingleNode(
                    "*[local-name() = 'ChargeTotalAmount']"
                );
                if (totalChargesNode != null)
                {
                    chargeSum = XmlParsingHelpers.ExtractDecimal(totalChargesNode);
                }

                var paidAmount = default(decimal?);
                var paidAmountNode = MonetarySummationNode.SelectSingleNode(
                    "*[local-name() = 'TotalPrepaidAmount']"
                );
                if (paidAmountNode != null)
                {
                    paidAmount = XmlParsingHelpers.ExtractDecimal(paidAmountNode);
                }

                var roundingAmount = default(decimal?);
                var roundingAmountNode = MonetarySummationNode.SelectSingleNode(
                    "*[local-name() = 'RoundingAmount']"
                );
                if (roundingAmountNode != null)
                {
                    roundingAmount = XmlParsingHelpers.ExtractDecimal(roundingAmountNode);
                }

                if (
                    _conformanceLevelType == FacturXConformanceLevelType.BasicWL
                    || _conformanceLevelType == FacturXConformanceLevelType.Basic
                )
                {
                    return new Models.BasicWL.Totals
                    {
                        TotalVatAmount = totalVatAmount,
                        TotalAmountWithVat = GetTotalAmountWithVat(),
                        TotalAmountWithoutVat = GetTotalAmountWithoutVat(),
                        AmountToBePaid = amountToBePaid,
                        TotalVatAmountInCurrency = GetTotalVatAmountInAccountingCurrency(
                            invoiceCurrency
                        ),
                        NetAmountSum = netAmountSum,
                        AllowancesSum = allowanceSum,
                        ChargesSum = chargeSum,
                        PaidAmount = paidAmount,
                    };
                }
                else if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
                {
                    return new Models.EN16931.Totals
                    {
                        TotalVatAmount = totalVatAmount,
                        TotalAmountWithVat = GetTotalAmountWithVat(),
                        TotalAmountWithoutVat = GetTotalAmountWithoutVat(),
                        AmountToBePaid = amountToBePaid,
                        TotalVatAmountInCurrency = GetTotalVatAmountInAccountingCurrency(
                            invoiceCurrency
                        ),
                        NetAmountSum = netAmountSum,
                        AllowancesSum = allowanceSum,
                        ChargesSum = chargeSum,
                        PaidAmount = paidAmount,
                        RoundingAmount = roundingAmount,
                    };
                }
                else
                {
                    return new Models.Extended.Totals
                    {
                        TotalVatAmount = totalVatAmount,
                        TotalAmountWithVat =
                            GetTotalAmountWithVat() as Models.Minimum.TotalAmountAndCurrency,
                        TotalAmountWithoutVat =
                            GetTotalAmountWithoutVat() as Models.Minimum.TotalAmountAndCurrency,
                        AmountToBePaid = amountToBePaid,
                        TotalVatAmountInCurrency = GetTotalVatAmountInAccountingCurrency(
                            invoiceCurrency
                        ),
                        NetAmountSum = netAmountSum,
                        AllowancesSum = allowanceSum,
                        ChargesSum = chargeSum,
                        PaidAmount = paidAmount,
                        RoundingAmount = roundingAmount,
                    };
                }
            }
        }

        #endregion
        protected Models.BasicWL.AllowanceCharge GetAllowanceCharge(XmlNode allowanceChargeNode)
        {
            var chargeIndicatorNode = allowanceChargeNode
                .SelectSingleNode("*[local-name() = 'ChargeIndicator']")
                ?.SelectSingleNode("*[local-name() = 'Indicator']");

            var chargeIndicator = XmlParsingHelpers.ExtractBool(chargeIndicatorNode);

            var percentage = default(decimal?);
            var percentageNode = allowanceChargeNode.SelectSingleNode(
                "*[local-name() = 'CalculationPercent']"
            );
            if (percentageNode != null)
            {
                percentage = XmlParsingHelpers.ExtractDecimal(percentageNode);
            }

            var baseAmount = default(decimal?);
            var baseAmountNode = allowanceChargeNode.SelectSingleNode(
                "*[local-name() = 'BasisAmount']"
            );
            if (baseAmountNode != null)
            {
                baseAmount = XmlParsingHelpers.ExtractDecimal(baseAmountNode);
            }

            var actualAmount = default(decimal?);
            var actualAmountNode = allowanceChargeNode.SelectSingleNode(
                "*[local-name() = 'ActualAmount']"
            );
            if (actualAmountNode != null)
            {
                actualAmount = XmlParsingHelpers.ExtractDecimal(actualAmountNode);
            }

            var reason = new Reason
            {
                Text = allowanceChargeNode
                    .SelectSingleNode("*[local-name() = 'Reason']")
                    ?.InnerText,
                Code = allowanceChargeNode
                    .SelectSingleNode("*[local-name() = 'ReasonCode']")
                    ?.InnerText,
            };

            var categoryTradeTax = allowanceChargeNode.SelectSingleNode(
                "*[local-name() = 'CategoryTradeTax']"
            );

            var vatRate = default(decimal?);
            var vatRateNode = categoryTradeTax?.SelectSingleNode(
                "*[local-name() = 'RateApplicablePercent']"
            );
            if (vatRateNode != null)
            {
                vatRate = XmlParsingHelpers.ExtractDecimal(vatRateNode);
            }

            var vatType = categoryTradeTax
                ?.SelectSingleNode("*[local-name() = 'TypeCode']")
                ?.InnerText;

            var vatCategoryCode = categoryTradeTax
                ?.SelectSingleNode("*[local-name() = 'CategoryCode']")
                ?.InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var sequenceNumeric = allowanceChargeNode
                    .SelectSingleNode("*[local-name() = 'SequenceNumeric']")
                    ?.InnerText;

                var baseQuantity = default(QuantityUnit);

                var baseQuantityNode = allowanceChargeNode.SelectSingleNode(
                    "*[local-name() = 'BasisQuantity']"
                );
                if (baseQuantityNode != null)
                {
                    baseQuantity = new QuantityUnit
                    {
                        Quantity = XmlParsingHelpers.ExtractDecimal(baseQuantityNode),
                        UnitCode = XmlParsingHelpers.ExtractAttribute(baseQuantityNode, "unitCode"),
                    };
                }

                return new Models.Extended.AllowanceCharge
                {
                    ChargeIndicator = chargeIndicator,
                    ActualAmount = actualAmount,
                    Percentage = percentage,
                    BaseAmount = baseAmount,
                    Reason = reason,
                    VatType = vatType,
                    VatCategory = vatCategoryCode,
                    VatRate = vatRate,
                    SequenceNumber = sequenceNumeric,
                    BaseQuantity = baseQuantity,
                };
            }

            return new Models.BasicWL.AllowanceCharge
            {
                ChargeIndicator = chargeIndicator,
                ActualAmount = actualAmount,
                Percentage = percentage,
                BaseAmount = baseAmount,
                Reason = reason,
                VatType = vatType,
                VatCategory = vatCategoryCode,
                VatRate = vatRate,
            };
        }
    }
}
