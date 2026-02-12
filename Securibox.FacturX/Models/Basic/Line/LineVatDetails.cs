namespace Securibox.FacturX.Models.Basic
{
    public class LineVatDetails
    {
        public string? VatType { get; set; }

        public string? VatCategory { get; set; }

        public decimal? VatRate { get; set; }

        public LineVatDetails() { }

        public LineVatDetails(string? vatType, string? vatCategory, decimal? vatRate)
        {
            VatType = vatType;
            VatCategory = vatCategory;
            VatRate = vatRate;
        }
    }
}
