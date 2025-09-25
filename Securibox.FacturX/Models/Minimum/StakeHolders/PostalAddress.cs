namespace Securibox.FacturX.Models.Minimum
{
    public class PostalAddress
    {
        public string? Country { get; set; }

        public PostalAddress(string? countryCode) => Country = countryCode;
    }
}