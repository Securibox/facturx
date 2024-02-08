namespace Securibox.FacturX.Models.BasicWL
{
    public class ElectronicAddress
    {
        public string UriId { get; internal set; }

        public string? Scheme { get; internal set; }

        public ElectronicAddress(string uriId, string? scheme)
        {
            UriId = uriId;
            Scheme = scheme;
        }
    }
}