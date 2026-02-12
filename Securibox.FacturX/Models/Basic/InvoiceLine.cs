namespace Securibox.FacturX.Models.Basic
{
    public class InvoiceLine
    {
        public LineDetails LineDetails { get; set; }

        public LineItemDetails ItemDetails { get; set; }

        public LineGrossPriceDetails? GrossPriceDetails { get; set; }

        public LineNetPriceDetails? NetPriceDetails { get; set; }

        public LineDeliveryDetails DeliveryDetails { get; set; }

        public LineVatDetails? VatDetails { get; set; }

        public IEnumerable<LineAllowanceCharge>? ChargeList { get; set; }

        public IEnumerable<LineAllowanceCharge>? AllowanceList { get; set; }

        public LineTotals Totals { get; set; }
    }
}
