using Securibox.FacturX.Models.Basic;
using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Extended;
using Securibox.FacturX.Models.Extended.Payment;
using System.Globalization;
using System.Xml;

namespace Securibox.FacturX.Core.Line
{
    internal class ExtendedLineBuilder : LineBuilder
    {
        private Models.Extended.InvoiceLine line;

        internal ExtendedLineBuilder(TradePartyFactory tradePartyFactory, ReferenceFactory referenceFactory) 
            : base(FacturXConformanceLevelType.Extended, tradePartyFactory, referenceFactory)
        {
        }

        internal void Reset(XmlNode lineNode)
        {
            base.ResetAll(lineNode);
            line = new Models.Extended.InvoiceLine();
        }

        internal override void BuildLineAllowanceAndChargeList()
        {
            var allowanceChargeList = GetAllowanceAndChargeList()?.Cast<Models.EN16931.LineAllowanceCharge>();
            if (allowanceChargeList == null)
                return;

            var allowanceList = allowanceChargeList.Where(x => x.ChargeIndicator == false).ToList();
            line.AllowanceList = allowanceList.Any() ? allowanceList : null;

            var chargeList = allowanceChargeList.Where(x => x.ChargeIndicator == true).ToList();
            line.ChargeList = chargeList.Any() ? chargeList : null;
        }

        internal override void BuildLineDeliveryDetails()
        {
            var deliveryDetails = new Models.Extended.LineDeliveryDetails
            {
                InvoicedQuantity = GetInvoicedQuantity(),
                DeliveryPeriod = GetDeliveryPeriod(),
                DeliverToAddress = TradePartyFactory.ConvertLineActor(LineNode, TradePartyType.DeliverToAddress) as Models.Extended.LineDeliverAddress,
                UltimateDeliverToAddress = TradePartyFactory.ConvertLineActor(LineNode, TradePartyType.UltimateDeliverToAddress) as Models.Extended.LineDeliverAddress,
                DespatchAdviceReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.DespatchAdviceReference) as Models.Extended.LineDespacthAdviceReference,
                ReceivingAdviceReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.ReceivingAdviceReference) as Models.Extended.LineReceivingAdviceReference,
                DeliveryNoteReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.DeliveryNoteReference) as Models.Extended.LineDeliveryNoteReference,
                DeliveryDate = GetDeliveryDate(),
                ChargeFreeQuantity = GetChargeFreeQuantity(),
                PackageQuantity = GetPackageQuantity(),
            };

            line.DeliveryDetails = deliveryDetails;
        }

        #region Delivery
        private QuantityUnit? GetChargeFreeQuantity()
        {
            var chargeFreeQuantityNode = TradeDeliveryNode.SelectSingleNode("*[local-name() = 'ChargeFreeQuantity']");
            if (chargeFreeQuantityNode == null)
                return null;

            var quantity = XmlParsingHelpers.ExtractDecimal(chargeFreeQuantityNode);
            var unitCode = chargeFreeQuantityNode.Attributes!["unitCode"]!.Value;

            return new QuantityUnit
            {
                Quantity = quantity,
                UnitCode = unitCode,
            };
        }

        private QuantityUnit? GetPackageQuantity()
        {
            var packageQuantityNode = TradeDeliveryNode.SelectSingleNode("*[local-name() = 'PackageQuantity']");
            if (packageQuantityNode == null)
                return null;

            var quantity = XmlParsingHelpers.ExtractDecimal(packageQuantityNode);
            var unitCode = packageQuantityNode.Attributes!["unitCode"]!.Value;

            return new QuantityUnit
            {
                Quantity = quantity,
                UnitCode = unitCode,
            };
        }

        private DateTime? GetDeliveryDate()
        {
            var actualDeliveryDateNode = TradeDeliveryNode.SelectSingleNode("*[local-name() = 'ActualDeliverySupplyChainEvent']");
            if (actualDeliveryDateNode == null)
                return null;

            var occurrenceDateNode = actualDeliveryDateNode
                .SelectSingleNode("*[local-name() = 'OccurrenceDateTime']")!
                .SelectSingleNode("*[local-name() = 'DateTimeString']")!;

            return XmlParsingHelpers.ExtractDateTime(occurrenceDateNode);
        }


        private IEnumerable<ShippingTransportation>? GetShippingTransportationList()
        {
            var relatedSupplyChainConsignment = TradeDeliveryNode.SelectSingleNode("*[local-name() = 'RelatedSupplyChainConsignment']");
            if (relatedSupplyChainConsignment == null)
                return null;

            var shippingTransportationNodeList = relatedSupplyChainConsignment.SelectNodes("*[local-name() = 'SpecifiedLogisticsTransportMovement']");
            if (shippingTransportationNodeList == null)
                return null;

            var shippingTransportationList = new List<ShippingTransportation>();
            foreach (XmlNode shippingTransportationNode in shippingTransportationNodeList)
            {
                var shippingTransportation = new ShippingTransportation
                {
                    ModeCode = shippingTransportationNode?.SelectSingleNode("*[local-name() = 'ModeCode']")?.InnerText,
                };

                shippingTransportationList.Add(shippingTransportation);
            }

            return shippingTransportationList;
        }
        #endregion

        internal override void BuildLineDetails()
        {
            line.LineDetails = GetLineDetails() as Models.Extended.LineDetails;
        }

        internal override void BuildItemDetails()
        {
            line.ItemDetails = GetItemDetails() as Models.Extended.LineItemDetails;
        }

        internal override void BuildLineGrossPriceDetails()
        {
            line.GrossPriceDetails = GetGrossPriceDetails() as Models.Extended.LineGrossPriceDetails;
        }

        internal override void BuildLineNetPriceDetails()
        {
            var netPriceDetails = new Models.Extended.LineNetPriceDetails
            {
                NetPrice = GetNetPriceAmount(),
                BaseQuantity = GetNetPriceBaseQuantity(),
                IncludedTax = GetNetPriceIncludedTax()
            };

            line.NetPriceDetails = netPriceDetails;
        }

        #region NetPrice
        private IncludedTax? GetNetPriceIncludedTax()
        {
            var includedTaxNode = NetPriceNode?.SelectSingleNode("*[local-name() = 'IncludedTradeTax']");
            if (includedTaxNode == null)
                return null;

            var vatAmountNode = includedTaxNode.SelectSingleNode("*[local-name() = 'CalculatedAmount']")!;
            var vatAmount = XmlParsingHelpers.ExtractDecimal(vatAmountNode);

            var vatType = includedTaxNode.SelectSingleNode("*[local-name() = 'TypeCode']")!.InnerText;

            var reason = new Reason
            {
                Text = includedTaxNode.SelectSingleNode("*[local-name() = 'ExemptionReason']")?.InnerText,
                Code = includedTaxNode.SelectSingleNode("*[local-name() = 'ExemptionReasonCode']")?.InnerText,
            };

            var vatCategoryCode = includedTaxNode.SelectSingleNode("*[local-name() = 'CategoryCode']")!.InnerText;

            var vatRateNode = includedTaxNode.SelectSingleNode("*[local-name() = 'RateApplicablePercent']")!;
            var vatRate = XmlParsingHelpers.ExtractDecimal(vatRateNode);

            return new IncludedTax
            {
                VatAmount = vatAmount,
                VatType = vatType,
                VatExemptionReason = reason,
                VatCategoryCode = vatCategoryCode,
                VatRate = vatRate,
            };
        }
        #endregion

        internal override void BuildLineTotals()
        {
            var totalAllowance = default(decimal?);
            var totalAllowanceNode = MonetarySummationNode.SelectSingleNode("*[local-name() = 'AllowanceTotalAmount']");
            if (totalAllowanceNode != null)
            {
                totalAllowance = XmlParsingHelpers.ExtractDecimal(totalAllowanceNode);
            }

            var totalCharge = default(decimal?);
            var totalChargeNode = MonetarySummationNode.SelectSingleNode("*[local-name() = 'ChargeTotalAmount']");
            if (totalChargeNode != null)
            {
                totalCharge = XmlParsingHelpers.ExtractDecimal(totalChargeNode);
            }

            var totalVatAmount = default(decimal?);
            var totalVatAmountNode = MonetarySummationNode.SelectSingleNode("*[local-name() = 'TaxTotalAmount']");
            if (totalVatAmountNode != null)
            {
                totalVatAmount = XmlParsingHelpers.ExtractDecimal(totalVatAmountNode);
            }

            var grandTotalAmount = default(decimal?);
            var grandTotalAmountNode = MonetarySummationNode.SelectSingleNode("*[local-name() = 'GrandTotalAmount']");
            if (grandTotalAmountNode != null)
            {
                grandTotalAmount = XmlParsingHelpers.ExtractDecimal(grandTotalAmountNode);
            }

            var totalAllowanceAndCharge = default(decimal?);
            var totalAllowanceAndChargeNode = MonetarySummationNode.SelectSingleNode("*[local-name() = 'TotalAllowanceChargeAmount']");
            if (totalAllowanceAndChargeNode != null)
            {
                totalAllowanceAndCharge = XmlParsingHelpers.ExtractDecimal(totalAllowanceAndChargeNode);
            }

            var totals = new Models.Extended.LineTotals
            {
                LineNetAmount = GetNetAmount(),
                TotalAllowanceAndCharge = totalAllowanceAndCharge,
                TotalAllowanceWithoutVat = totalAllowance,
                TotalAmountWithVat = grandTotalAmount,
                TotalChargeWithoutVat = totalCharge,
                TotalVatAmount = totalVatAmount,
            };

            line.Totals = totals;
        }

        internal override void BuildLineVatDetails()
        {
            var vatDetails = GetVatDetails() as Models.Extended.LineVatDetails;
            if (vatDetails == null)
                return;

            line.VatDetails = vatDetails;
        }

        internal override void BuildLineItemAttributeList() 
        {
            line.ItemAttributeList = GetItemAttributeList()?.Cast<Models.Extended.LineItemAttribute>();
        }

        internal override void BuildLineItemClassificationList() 
        {
            line.ItemClassificationList = GetItemClassificationList()?.Cast<Models.Extended.LineItemClassification>();
        }

        internal override void BuildLineReferences()
        {
            line.BuyerAccountingReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.BuyerAccountingReference) as Models.Extended.BuyerAccountingReference;
            line.InvoicedObjectIdentifier = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.InvoicedObjectIdentifier) as Models.Extended.InvoicedObjectIdentifier;
            line.PurchaseOrderReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.PurchaseOrderReference) as Models.Extended.LinePurchaseOrderReference;
            line.ContractReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.ContractReference) as Models.Extended.LineContractReference;
            line.QuotationReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.QuotationReference) as Models.Extended.LineQuotationReference;
            line.UltimateCustomerOrderReferenceList = ReferenceFactory.ConvertLineReferenceList(LineNode, References.ReferenceType.UltimateCustomerOrderReferenceList)?.Cast<Models.Extended.LineUltimateCustomerOrderReference>();
            line.AdditionalDocumentReferenceList = ReferenceFactory.ConvertLineReferenceList(LineNode, References.ReferenceType.LineAdditionalDocumentReferenceList)?.Cast<Models.Extended.LineAdditionalDocument>();
        }

        internal override void BuildLineIncludedItemList() 
        {
            if (IncludedItemNodeList == null || IncludedItemNodeList.Count == 0)
                return;

            var includedItemList = new List<LineIncludedItem>();
            foreach (XmlNode includedItemNode in IncludedItemNodeList)
                includedItemList.Add(GetIncludedItem(includedItemNode));

            line.IncludedItemList = includedItemList;
        }

        private Models.Extended.LineIncludedItem GetIncludedItem(XmlNode includedItemNode)
        {
            var name = includedItemNode.SelectSingleNode("*[local-name() = 'Name']")!.InnerText;

            var includedItem = new Models.Extended.LineIncludedItem(name);

            var id = includedItemNode.SelectSingleNode("*[local-name() = 'ID']")?.InnerText;
            var buyerAssignedId = includedItemNode.SelectSingleNode("*[local-name() = 'BuyerAssignedID']")?.InnerText;
            var sellerAssignedId = includedItemNode.SelectSingleNode("*[local-name() = 'SellerAssignedID']")?.InnerText;
            var industryAssignedId = includedItemNode.SelectSingleNode("*[local-name() = 'IndustryAssignedID']")?.InnerText;
            var description = includedItemNode.SelectSingleNode("*[local-name() = 'Description']")?.InnerText;

            includedItem.AddId(id);
            includedItem.AddBuyerAssignedId(buyerAssignedId);
            includedItem.AddSellerAssignedId(sellerAssignedId);
            includedItem.AddIndustryAssignedId(industryAssignedId);
            includedItem.AddDescription(description);

            var globalIdNodeList = includedItemNode.SelectNodes("*[local-name() = 'GlobalID']");
            if (globalIdNodeList != null && globalIdNodeList.Count > 0)
            {
                var globalIdList = new List<GlobalIdentification>();
                foreach (XmlNode globalIdNode in globalIdNodeList)
                {
                    var globalId = globalIdNode.InnerText;
                    var scheme = globalIdNode.Attributes!["schemeID"]!.Value;
                    globalIdList.Add(new GlobalIdentification(globalId, scheme));
                }

                includedItem.AddGlobalIdentificationList(globalIdList);
            }

            var unitQuantityNode = includedItemNode.SelectSingleNode("*[local-name() = 'UnitQuantity']");
            if (unitQuantityNode != null)
            {
                decimal.TryParse(unitQuantityNode.InnerText, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal quantity);
                var unitCode = unitQuantityNode.Attributes!["unitCode"]!.Value;

                includedItem.AddUnitQuantity(new QuantityUnit(quantity, unitCode));
            }

            return includedItem;
        }

        internal override void BuildLineItemInstanceList() 
        {
            if (ItemInstanceNodeList == null || ItemInstanceNodeList.Count == 0)
                return;

            var itemInstanceList = new List<LineItemInstance>();
            foreach (XmlNode itemInstanceNode in ItemInstanceNodeList)
            {
                var batchId = itemInstanceNode.SelectSingleNode("*[local-name() = 'BatchID']")?.InnerText;
                var supplierSerialId = itemInstanceNode.SelectSingleNode("*[local-name() = 'SupplierAssignedSerialID']")?.InnerText;
                
                itemInstanceList.Add(new LineItemInstance(batchId, supplierSerialId));
            }

            line.ItemInstanceList = itemInstanceList;
        }

        internal Models.Extended.InvoiceLine GetLine()
        {
            BuildLineDetails();
            BuildLineGrossPriceDetails();
            BuildLineNetPriceDetails();
            BuildLineAllowanceAndChargeList();
            BuildLineDeliveryDetails();
            BuildItemDetails();
            BuildLineVatDetails();
            BuildLineTotals();
            BuildLineItemAttributeList();
            BuildLineItemClassificationList();
            BuildLineReferences();
            BuildLineIncludedItemList();
            BuildLineItemInstanceList();

            return line;
        }

    }
}
