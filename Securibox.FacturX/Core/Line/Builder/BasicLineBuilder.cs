using System.Xml;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Core.Line
{
    internal class BasicLineBuilder : LineBuilder
    {
        private Models.Basic.InvoiceLine line;

        internal BasicLineBuilder(
            TradePartyFactory tradePartyFactory,
            ReferenceFactory referenceFactory
        )
            : base(FacturXConformanceLevelType.Basic, tradePartyFactory, referenceFactory) { }

        internal void Reset(XmlNode lineNode)
        {
            base.ResetAll(lineNode);
            line = new Models.Basic.InvoiceLine();
        }

        internal override void BuildLineAllowanceAndChargeList()
        {
            var allowanceChargeList = GetAllowanceAndChargeList();
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
            line.ItemDetails = GetItemDetails();
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
            var totals = new Models.Basic.LineTotals { LineNetAmount = GetNetAmount() };

            line.Totals = totals;
        }

        internal override void BuildLineVatDetails()
        {
            var vatDetails = GetVatDetails();
            if (vatDetails == null)
                return;

            line.VatDetails = vatDetails;
        }

        internal Models.Basic.InvoiceLine GetLine()
        {
            BuildLineDetails();
            BuildLineGrossPriceDetails();
            BuildLineNetPriceDetails();
            BuildLineAllowanceAndChargeList();
            BuildLineDeliveryDetails();
            BuildItemDetails();
            BuildLineVatDetails();
            BuildLineTotals();

            return line;
        }
    }
}
