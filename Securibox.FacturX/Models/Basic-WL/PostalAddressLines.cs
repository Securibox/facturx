namespace Securibox.FacturX.Models.BasicWL
{
    public class PostalAddressLines
    {
        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public PostalAddressLines(string? addressLine1)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = null;
            AddressLine3 = null;
        }

        public PostalAddressLines(string? addressLine1, string? addressLine2)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = null;
        }

        public PostalAddressLines(string? addressLine1, string? addressLine2, string? addressLine3)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = addressLine3;
        }
    }
}
