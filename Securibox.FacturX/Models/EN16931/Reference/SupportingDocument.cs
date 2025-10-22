namespace Securibox.FacturX.Models.EN16931
{
    public class SupportingDocument : AdditionalDocument
    {
        public string? ExternalLocation { get; internal set; }

        public string? Description { get; set; }

        public Attachment? AttachedDocument { get; internal set; }
    }
}
