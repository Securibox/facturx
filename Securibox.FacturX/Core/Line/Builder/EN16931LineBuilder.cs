using Securibox.FacturX.Models.Enums;
using System.Xml;

namespace Securibox.FacturX.Core.Line
{
    internal class EN16931LineBuilder : LineBuilder
    {
        private Models.EN16931.InvoiceLine line;

        internal EN16931LineBuilder(TradePartyFactory tradePartyFactory, ReferenceFactory referenceFactory) 
            : base(FacturXConformanceLevelType.EN16931, tradePartyFactory, referenceFactory)
        {
        }

        internal void Reset(XmlNode lineNode)
        {
            base.ResetAll(lineNode);
            line = new Models.EN16931.InvoiceLine();
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
            var deliveryDetails = new Models.Basic.LineDeliveryDetails
            {
                InvoicedQuantity = GetInvoicedQuantity(),
                DeliveryPeriod = GetDeliveryPeriod(),
            };

            line.DeliveryDetails = deliveryDetails;
        }

        internal override void BuildLineDetails()
        {
            line.LineDetails = GetLineDetails();
        }

        internal override void BuildItemDetails()
        {
            line.ItemDetails = GetItemDetails() as Models.EN16931.LineItemDetails;
        }

        internal override void BuildLineGrossPriceDetails()
        {
            if (GrossPriceNode == null)
                return;

            line.GrossPriceDetails = GetGrossPriceDetails();
        }

        internal override void BuildLineNetPriceDetails()
        {
            var netPriceDetails = new Models.Basic.LineNetPriceDetails
            {
                NetPrice = GetNetPriceAmount(),
                BaseQuantity = GetNetPriceBaseQuantity(),
            };

            line.NetPriceDetails = netPriceDetails;
        }

        internal override void BuildLineTotals()
        {
            var totals = new Models.Basic.LineTotals
            {
                LineNetAmount = GetNetAmount(),
            };

            line.Totals = totals;
        }

        internal override void BuildLineVatDetails()
        {
            var vatDetails = GetVatDetails();
            if (vatDetails == null)
                return;

            line.VatDetails = vatDetails;
        }

        internal override void BuildLineItemAttributeList() 
        {
            line.ItemAttributeList = GetItemAttributeList();
        }

        internal override void BuildLineItemClassificationList() 
        {
            line.ItemClassificationList = GetItemClassificationList();
        }

        internal override void BuildLineReferences() 
        {
            line.BuyerAccountingReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.BuyerAccountingReference) as Models.BasicWL.BuyerAccountingReference;
            line.InvoicedObjectIdentifier = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.InvoicedObjectIdentifier) as Models.EN16931.InvoicedObjectIdentifier;
            line.PurchaseOrderReference = ReferenceFactory.ConvertLineReference(LineNode, References.ReferenceType.PurchaseOrderReference) as Models.EN16931.LinePurchaseOrderReference;
        }

        internal Models.EN16931.InvoiceLine GetLine()
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

            return line;
        }
    }
}
