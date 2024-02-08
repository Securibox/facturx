using Securibox.FacturX.Models.Basic;
using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Extended;
using System.Globalization;
using System.Xml;

namespace Securibox.FacturX.Core.Line
{
    internal abstract class LineBuilder
    {
        private readonly FacturXConformanceLevelType _conformanceLevelType;

        protected readonly ReferenceFactory ReferenceFactory;
        protected readonly TradePartyFactory TradePartyFactory;

        protected XmlNode LineNode { get; private set; }
        protected XmlNode AssociatedDocumentNode { get; private set; }
        protected XmlNode NetPriceNode { get; private set; }
        protected XmlNode? GrossPriceNode { get; private set; }
        protected XmlNode TradeSettlementNode { get; private set; }
        protected XmlNode TradeDeliveryNode { get; private set; }
        protected XmlNode ItemNode { get; private set; }
        protected XmlNode VatDetailsNode { get; private set; }
        protected XmlNodeList? AllowanceChargeNodeList { get; private set; }
        protected XmlNodeList? ItemClassificationNodeList { get; private set; }
        protected XmlNodeList? ItemAttributeNodeList { get; private set; }
        protected XmlNodeList? ItemInstanceNodeList { get; private set; }
        protected XmlNodeList? IncludedItemNodeList { get; private set; }
        protected XmlNode MonetarySummationNode { get; private set; }

        internal LineBuilder(FacturXConformanceLevelType conformanceLevelType, TradePartyFactory tradePartyFactory, ReferenceFactory referenceFactory)
        {
            _conformanceLevelType = conformanceLevelType;

            TradePartyFactory = tradePartyFactory;
            ReferenceFactory = referenceFactory;
        }

        internal void ResetAll(XmlNode lineNode)
        {
            LineNode = lineNode;

            AssociatedDocumentNode = lineNode.SelectSingleNode("*[local-name() = 'AssociatedDocumentLineDocument']")!;

            var tradeAgreementNode = lineNode.SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!;

            GrossPriceNode = tradeAgreementNode.SelectSingleNode("*[local-name() = 'GrossPriceProductTradePrice']");
            NetPriceNode = tradeAgreementNode.SelectSingleNode("*[local-name() = 'NetPriceProductTradePrice']")!;

            ItemNode = lineNode.SelectSingleNode("*[local-name() = 'SpecifiedTradeProduct']")!;

            ItemAttributeNodeList = ItemNode.SelectNodes("*[local-name() = 'ApplicableProductCharacteristic']");
            ItemInstanceNodeList = ItemNode.SelectNodes("*[local-name() = 'IndividualTradeProductInstance']");
            ItemClassificationNodeList = ItemNode.SelectNodes("*[local-name() = 'DesignatedProductClassification']");
            IncludedItemNodeList = ItemNode.SelectNodes("*[local-name() = 'IncludedReferencedProduct']");

            TradeDeliveryNode = lineNode.SelectSingleNode("*[local-name() = 'SpecifiedLineTradeDelivery']")!;

            TradeSettlementNode = lineNode.SelectSingleNode("*[local-name() = 'SpecifiedLineTradeSettlement']")!;

            AllowanceChargeNodeList = TradeSettlementNode.SelectNodes("*[local-name() = 'SpecifiedTradeAllowanceCharge']");
            VatDetailsNode = TradeSettlementNode.SelectSingleNode("*[local-name() = 'ApplicableTradeTax']")!;
            MonetarySummationNode = TradeSettlementNode.SelectSingleNode("*[local-name() = 'SpecifiedTradeSettlementLineMonetarySummation']")!;
        }

        internal abstract void BuildLineDetails();
        internal abstract void BuildItemDetails();
        internal abstract void BuildLineGrossPriceDetails();
        internal abstract void BuildLineNetPriceDetails();
        internal abstract void BuildLineAllowanceAndChargeList();
        internal abstract void BuildLineDeliveryDetails();
        internal abstract void BuildLineVatDetails();
        internal abstract void BuildLineTotals();
        internal virtual void BuildLineItemAttributeList() { }
        internal virtual void BuildLineItemClassificationList() { }
        internal virtual void BuildLineReferences() { }
        internal virtual void BuildLineIncludedItemList() { }
        internal virtual void BuildLineItemInstanceList() { }


        #region LineDetails
        private Models.Basic.LineNote? GetLineNote()
        {
            var noteNode = AssociatedDocumentNode.SelectSingleNode("*[local-name() = 'IncludedNote']");
            if (noteNode == null)
                return null;

            var content = noteNode.SelectSingleNode("*[local-name() = 'Content']")?.InnerText;
            var subjectCode = noteNode.SelectSingleNode("*[local-name() = 'SubjectCode']")?.InnerText;
            var contentCode = noteNode.SelectSingleNode("*[local-name() = 'ContentCode']")?.InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
                return new Models.Extended.LineNote(content, contentCode, subjectCode);

            return new Models.Basic.LineNote(content);
        }

        protected Models.Basic.LineDetails GetLineDetails()
        {
            var lineId = AssociatedDocumentNode.SelectSingleNode("*[local-name() = 'LineID']")!.InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var parentLineId = AssociatedDocumentNode.SelectSingleNode("*[local-name() = 'ParentLineID']")?.InnerText;
                var statusCode = AssociatedDocumentNode.SelectSingleNode("*[local-name() = 'LineStatusCode']")?.InnerText;
                var statusReasonCode = AssociatedDocumentNode?.SelectSingleNode("*[local-name() = 'LineStatusReasonCode']")?.InnerText;

                var extendedLineDetails = new Models.Extended.LineDetails(lineId, parentLineId, statusCode, statusReasonCode);
                extendedLineDetails.AddExtendedNote(GetLineNote() as Models.Extended.LineNote);

                return extendedLineDetails;
            }

            var basicLineDetails = new Models.Basic.LineDetails(lineId);
            basicLineDetails.AddBasicNote(GetLineNote());

            return basicLineDetails;
        }
        #endregion

        #region GrossPriceDetails
        private Models.Basic.PriceAllowanceCharge GetGrossPriceAllowanceCharge(XmlNode priceAllowanceCharge)
        {
            var chargeIndicatorString = priceAllowanceCharge
                .SelectSingleNode("*[local-name() = 'ChargeIndicator']")!
                .SelectSingleNode("*[local-name() = 'Indicator']")!
                .InnerText;

            bool.TryParse(chargeIndicatorString, out bool chargeIndicator);

            var actualAmount = default(decimal?);
            var actualAmountNode = priceAllowanceCharge.SelectSingleNode("*[local-name() = 'ActualAmount']");
            if (actualAmountNode != null)
            {
                decimal.TryParse(actualAmountNode.InnerText, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal actualAmountValue);
                actualAmount = actualAmountValue;
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var percentage = default(decimal?);
                var percentageNode = priceAllowanceCharge.SelectSingleNode("*[local-name() = 'CalculationPercent']");
                if (percentageNode != null)
                {
                    decimal.TryParse(percentageNode.InnerText, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal percentageValue);
                    percentage = percentageValue;
                }

                var baseAmount = default(decimal?);
                var baseAmountNode = priceAllowanceCharge.SelectSingleNode("*[local-name() = 'BasisAmount']");
                if (baseAmountNode != null)
                {
                    decimal.TryParse(baseAmountNode.InnerText, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal baseAmountValue);
                    baseAmount = baseAmountValue;
                }

                var reason = new Reason
                {
                    Text = priceAllowanceCharge.SelectSingleNode("*[local-name() = 'Reason']")?.InnerText,
                    Code = priceAllowanceCharge.SelectSingleNode("*[local-name() = 'ReasonCode']")?.InnerText,
                };

                return new Models.Extended.PriceAllowanceCharge
                {
                    ActualAmount = actualAmount,
                    ChargeIndicator = chargeIndicator,
                    BaseAmount = baseAmount,
                    Reason = reason,
                    Percentage = percentage,
                };
            }

            return new Models.Basic.PriceAllowanceCharge
            {
                ActualAmount = actualAmount,
                ChargeIndicator = chargeIndicator,
            };
        }

        internal IEnumerable<Models.Basic.PriceAllowanceCharge>? GetGrossPriceAllowanceChargeList()
        {
            var allowanceChargeNodeList = GrossPriceNode!.SelectNodes("*[local-name() = 'AppliedTradeAllowanceCharge']");
            if (allowanceChargeNodeList == null)
                return null;

            var allowanceChargeList = new List<Models.Basic.PriceAllowanceCharge>();
            foreach (XmlNode allowanceChargeNode in allowanceChargeNodeList)
            {
                allowanceChargeList.Add(GetGrossPriceAllowanceCharge(allowanceChargeNode));
            }

            return allowanceChargeList;
        }

        protected Models.Basic.LineGrossPriceDetails? GetGrossPriceDetails()
        {
            if (GrossPriceNode == null)
                return null;

            var price = default(decimal?);
            var priceNode = GrossPriceNode.SelectSingleNode("*[local-name() = 'ChargeAmount']");
            if (priceNode != null)
            {
                price = XmlParsingHelpers.ExtractDecimal(priceNode);
            }

            var baseQuantity = default(QuantityUnit);
            var baseQuantityNode = GrossPriceNode.SelectSingleNode("*[local-name() = 'BasisQuantity']");
            if (baseQuantityNode != null)
            {
                baseQuantity = new QuantityUnit();
                baseQuantity.AddQuantity(XmlParsingHelpers.ExtractDecimal(baseQuantityNode));
                baseQuantity.AddUnitCode(baseQuantityNode.Attributes?["unitCode"]?.Value);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var extendedAllowanceChargeList = GetGrossPriceAllowanceChargeList()?.Cast<Models.Extended.PriceAllowanceCharge>();

                return new Models.Extended.LineGrossPriceDetails
                {
                    BaseQuantity = baseQuantity,
                    GrossPrice = price,
                    PriceDiscount = extendedAllowanceChargeList?.FirstOrDefault(x => x.ChargeIndicator == false),
                    PriceCharge = extendedAllowanceChargeList?.FirstOrDefault(x => x.ChargeIndicator == true),
                };
            }

            var allowanceChargeList = GetGrossPriceAllowanceChargeList();

            return new Models.Basic.LineGrossPriceDetails
            {
                BaseQuantity = baseQuantity,
                GrossPrice = price,
                PriceDiscount = allowanceChargeList?.FirstOrDefault(x => x.ChargeIndicator == false),
            };
        }
        #endregion

        #region NetPriceDetails
        internal decimal GetNetPriceAmount()
        {
            var price = NetPriceNode.SelectSingleNode("*[local-name() = 'ChargeAmount']")!;
            return XmlParsingHelpers.ExtractDecimal(price);
        }

        internal QuantityUnit? GetNetPriceBaseQuantity()
        {
            var baseQuantityNode = NetPriceNode?.SelectSingleNode("*[local-name() = 'BasisQuantity']");
            if (baseQuantityNode == null)
                return null;

            var baseQuantity = XmlParsingHelpers.ExtractDecimal(baseQuantityNode);

            var unitCode = default(string);
            if (baseQuantityNode.Attributes != null && baseQuantityNode.Attributes["unitCode"] != null)
            {
                unitCode = baseQuantityNode.Attributes["unitCode"]!.Value;
            }

            return new QuantityUnit
            {
                Quantity = baseQuantity,
                UnitCode = unitCode
            };
        }
        #endregion

        #region ItemDetails
        internal Models.Basic.LineItemDetails GetItemDetails()
        {
            var name = ItemNode.SelectSingleNode("*[local-name() = 'Name']")!.InnerText;

            var globalIdentification = default(GlobalIdentification);
            var globalIdNode = ItemNode.SelectSingleNode("*[local-name() = 'GlobalID']");
            if (globalIdNode != null)
            {
                var globalId = globalIdNode.InnerText;
                var schemeId = globalIdNode.Attributes!["schemeID"]!.Value;
                globalIdentification = new GlobalIdentification(globalId, schemeId);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Basic)
            {
                return new Models.Basic.LineItemDetails(name, globalIdentification);
            }
             
            var description = ItemNode.SelectSingleNode("*[local-name() = 'Description']")?.InnerText;
            var buyerAssignedId = ItemNode.SelectSingleNode("*[local-name() = 'BuyerAssignedID']")?.InnerText;
            var sellerAssignedId = ItemNode.SelectSingleNode("*[local-name() = 'SellerAssignedID']")?.InnerText;

            var originCountry = ItemNode
                .SelectSingleNode("*[local-name() = 'OriginTradeCountry']")?
                .SelectSingleNode("*[local-name() = 'ID']")?
                .InnerText;
                
            if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                var en16931ItemDetails = new Models.EN16931.LineItemDetails(name, globalIdentification);
                en16931ItemDetails.AddDescription(description);
                en16931ItemDetails.AddBuyerAssignedId(buyerAssignedId);
                en16931ItemDetails.AddSellerAssignedId(sellerAssignedId);
                en16931ItemDetails.AddOriginCountry(originCountry);

                return en16931ItemDetails;
            }


            var extendedItemDetails = new Models.Extended.LineItemDetails(name, globalIdentification);
            extendedItemDetails.AddDescription(description);
            extendedItemDetails.AddBuyerAssignedId(buyerAssignedId);
            extendedItemDetails.AddSellerAssignedId(sellerAssignedId);
            extendedItemDetails.AddOriginCountry(originCountry);

            var id = ItemNode.SelectSingleNode("*[local-name() = 'ID']")?.InnerText;
            extendedItemDetails.AddId(id);

            return extendedItemDetails;
        }
        #endregion

        #region ItemAttribute
        private Models.EN16931.LineItemAttribute GetItemAttribute(XmlNode itemAttributeNode)
        {
            var name = itemAttributeNode.SelectSingleNode("*[local-name() = 'Description']")!.InnerText;
            var value = itemAttributeNode.SelectSingleNode("*[local-name() = 'Value']")!.InnerText;

            if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.LineItemAttribute(name, value);
            }

            var itemAttribute = new Models.Extended.LineItemAttribute(name, value);

            var type = itemAttributeNode.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText;
            itemAttribute.AddType(type);

            var valueMeasureNode = itemAttributeNode.SelectSingleNode("*[local-name() = 'ValueMeasure']");
            if (valueMeasureNode != null)
            {
                decimal.TryParse(valueMeasureNode.InnerText, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal valueMeasure);
                var unitCode = valueMeasureNode.Attributes!["unitCode"]!.Value;

                itemAttribute.AddValueMeasure(new LineAttributeValueMeasure(valueMeasure, unitCode));
            }

            return itemAttribute;
        }

        protected IEnumerable<Models.EN16931.LineItemAttribute>? GetItemAttributeList()
        {
            if (ItemAttributeNodeList == null || ItemAttributeNodeList.Count == 0)
                return null;

            var itemAttributeList = new List<Models.EN16931.LineItemAttribute>();
            foreach (XmlNode itemAttributeNode in ItemAttributeNodeList)
                itemAttributeList.Add(GetItemAttribute(itemAttributeNode));

            return itemAttributeList;
        }
        #endregion

        #region ItemClassification
        private Models.EN16931.LineItemClassification GetItemClassification(XmlNode classificationNode)
        {
            var classificationIdentifier = default(Models.EN16931.LineItemClassificationIdentifier);
            var classificationIdNode = classificationNode.SelectSingleNode("*[local-name() = 'ClassCode']");
            if (classificationIdNode != null)
            {
                var classificationId = classificationIdNode.InnerText;
                var schemeId = classificationIdNode.Attributes!["listID"]!.Value;
                var schemeVersionId = classificationIdNode.Attributes["listVersionID"]?.Value;
                classificationIdentifier = new Models.EN16931.LineItemClassificationIdentifier(classificationId, schemeId, schemeVersionId);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.LineItemClassification(classificationIdentifier);
            }

            var classificationName = classificationNode.SelectSingleNode("*[local-name() = 'ClassName']")?.InnerText;
            return new Models.Extended.LineItemClassification(classificationIdentifier, classificationName);
        }

        protected IEnumerable<Models.EN16931.LineItemClassification>? GetItemClassificationList()
        {
            if (ItemClassificationNodeList == null || ItemClassificationNodeList.Count == 0)
                return null;

            var itemClassificationList = new List<Models.EN16931.LineItemClassification>();
            foreach (XmlNode itemClassificationNode in ItemClassificationNodeList)
                itemClassificationList.Add(GetItemClassification(itemClassificationNode));

            return itemClassificationList;
        }

        #endregion

        #region AllowanceCharge
        private Models.Basic.LineAllowanceCharge GetAllowanceCharge(XmlNode allowanceChargeNode)
        {
            var chargeIndicatorNode = allowanceChargeNode
                .SelectSingleNode("*[local-name() = 'ChargeIndicator']")!
                .SelectSingleNode("*[local-name() = 'Indicator']");

            var chargeIndicator = XmlParsingHelpers.ExtractBool(chargeIndicatorNode);


            var actualAmount = default(decimal?);
            var actualAmountNode = allowanceChargeNode.SelectSingleNode("*[local-name() = 'ActualAmount']");
            if (actualAmountNode != null)
            {
                actualAmount = XmlParsingHelpers.ExtractDecimal(actualAmountNode);
            }

            var reason = new Reason
            {
                Text = allowanceChargeNode.SelectSingleNode("*[local-name() = 'Reason']")?.InnerText,
                Code = allowanceChargeNode.SelectSingleNode("*[local-name() = 'ReasonCode']")?.InnerText,
            };

            if (_conformanceLevelType == FacturXConformanceLevelType.Basic)
            {
                return new Models.Basic.LineAllowanceCharge
                {
                    ChargeIndicator = chargeIndicator,
                    ActualAmount = actualAmount,
                    Reason = reason,
                };
            }

            var percentage = default(decimal?);
            var percentageNode = allowanceChargeNode.SelectSingleNode("*[local-name() = 'CalculationPercent']");
            if (percentageNode != null)
            {
                percentage = XmlParsingHelpers.ExtractDecimal(percentageNode);
            }

            var baseAmount = default(decimal?);
            var baseAmountNode = allowanceChargeNode.SelectSingleNode("*[local-name() = 'BasisAmount']");
            if (baseAmountNode != null)
            {
                baseAmount = XmlParsingHelpers.ExtractDecimal(baseAmountNode);
            }


            return new Models.EN16931.LineAllowanceCharge
            {
                ChargeIndicator = chargeIndicator,
                ActualAmount = actualAmount,
                Reason = reason,
                Percentage = percentage,
                BaseAmount = baseAmount,
            };

        }

        protected IEnumerable<Models.Basic.LineAllowanceCharge>? GetAllowanceAndChargeList()
        {
            if (AllowanceChargeNodeList == null || AllowanceChargeNodeList.Count == 0)
                return null;

            var allowanceChargeList = new List<Models.Basic.LineAllowanceCharge>();
            foreach (XmlNode allowanceChargeNode in AllowanceChargeNodeList)
            {
                allowanceChargeList.Add(GetAllowanceCharge(allowanceChargeNode));
            }

            return allowanceChargeList;
        }
        #endregion

        #region VatDetails
        protected Models.Basic.LineVatDetails GetVatDetails()
        {
            var vatType = VatDetailsNode.SelectSingleNode("*[local-name() = 'TypeCode']")!.InnerText;
            var vatCategoryCode = VatDetailsNode.SelectSingleNode("*[local-name() = 'CategoryCode']")!.InnerText;

            var vatRate = default(decimal?);
            var vatRateNode = VatDetailsNode.SelectSingleNode("*[local-name() = 'RateApplicablePercent']");
            if (vatRateNode != null)
            {
                vatRate = XmlParsingHelpers.ExtractDecimal(vatRateNode);
            }

            if (_conformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var vatAmount = default(decimal?);
                var vatAmountNode = VatDetailsNode.SelectSingleNode("*[local-name() = 'CalculatedAmount']");
                if (vatAmountNode != null)
                {
                    vatAmount = XmlParsingHelpers.ExtractDecimal(vatAmountNode);
                }

                var reason = new Reason
                {
                    Text = VatDetailsNode.SelectSingleNode("*[local-name() = 'ExemptionReason']")?.InnerText,
                    Code = VatDetailsNode.SelectSingleNode("*[local-name() = 'ExemptionReasonCode']")?.InnerText,
                };

                return new Models.Extended.LineVatDetails
                {
                    VatType = vatType,
                    VatCategory = vatCategoryCode,
                    VatRate = vatRate,
                    VatAmount = vatAmount,
                    VatExemptionReason = reason,
                };
            }

            return new Models.Basic.LineVatDetails
            {
                VatType = vatType,
                VatCategory = vatCategoryCode,
                VatRate = vatRate,
            };
        }
        #endregion

        #region Delivery
        protected QuantityUnit GetInvoicedQuantity()
        {
            var lineInvoicedQuantityNode = TradeDeliveryNode
                .SelectSingleNode("*[local-name() = 'BilledQuantity']")!;

            var quantity = XmlParsingHelpers.ExtractDecimal(lineInvoicedQuantityNode);
            var unitCode = lineInvoicedQuantityNode.Attributes!["unitCode"]!.Value;

            return new QuantityUnit
            {
                Quantity = quantity,
                UnitCode = unitCode,
            };
        }

        protected Models.BasicWL.DeliveryPeriod? GetDeliveryPeriod()
        {
            var deliveryPeriodNode = TradeSettlementNode.SelectSingleNode("*[local-name() = 'BillingSpecifiedPeriod']");

            if (deliveryPeriodNode == null)
                return null;

            var startDate = default(DateTime?);
            var startDateNode = deliveryPeriodNode.SelectSingleNode("*[local-name() = 'StartDateTime']");
            if (startDateNode != null)
            {
                var dateStringNode = startDateNode.SelectSingleNode("*[local-name() = 'DateTimeString']")!;
                startDate = XmlParsingHelpers.ExtractDateTime(dateStringNode);
            }

            var endDate = default(DateTime?);
            var endDateNode = deliveryPeriodNode.SelectSingleNode("*[local-name() = 'EndDateTime']");
            if (endDateNode != null)
            {
                var dateStringNode = endDateNode.SelectSingleNode("*[local-name() = 'DateTimeString']")!;
                endDate = XmlParsingHelpers.ExtractDateTime(dateStringNode);
            }

            return new Models.BasicWL.DeliveryPeriod(startDate, endDate);
        }
        #endregion

        #region Totals
        protected decimal GetNetAmount()
        {
            var lineTotalAmountNode = MonetarySummationNode.SelectSingleNode("*[local-name() = 'LineTotalAmount']")!;
            return XmlParsingHelpers.ExtractDecimal(lineTotalAmountNode);
        }
        #endregion
    }
}
