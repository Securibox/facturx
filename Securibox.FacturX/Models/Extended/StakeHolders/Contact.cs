namespace Securibox.FacturX.Models.Extended
{
    public class Contact : EN16931.Contact
    {
        public string? Type { get; set; }

        public string? FaxNumber { get; set; }
    }
}