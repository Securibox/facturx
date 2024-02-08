namespace Securibox.FacturX.Models.BasicWL
{
    public class PostalAddress : Minimum.PostalAddress
    {
        public string? PostCode { get; set; }
        public PostalAddressLines? AddressLines { get; set; }
        public string? City { get; set; }
        public string? CountrySubdivision { get; set; }

        public PostalAddress(string[] addressLines, string postCode, string city, string country):
            base(country)
        {
            AddressLines = new PostalAddressLines(addressLines[0])
            {
                AddressLine2 = addressLines.Length > 1 ? addressLines[1] : null,
                AddressLine3 = addressLines.Length > 2 ? addressLines[2] : null,
            };
        }

        public PostalAddress(Minimum.PostalAddress postalAddress, string? postCode, PostalAddressLines? addressLines, string? city, string? countrySubdivision)
            : base(postalAddress.Country)
        {
            PostCode = postCode;
            AddressLines = addressLines;
            City = city;
            CountrySubdivision = countrySubdivision;
        }
    }
}
